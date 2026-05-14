using System.Text.Json;
using AquaSense.Backend.Hubs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Services;
using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;

namespace AquaSense.Backend.Mqtt;

public class MqttWorker : BackgroundService
{
    private readonly IHubContext<SensorHub> _hubContext;
    private readonly SupabaseWriter _supabaseWriter;
    private readonly IConfiguration _config;
    private readonly ILogger<MqttWorker> _logger;

    public MqttWorker(
        IHubContext<SensorHub> hubContext,
        SupabaseWriter supabaseWriter,
        IConfiguration config,
        ILogger<MqttWorker> logger)
    {
        _hubContext = hubContext;
        _supabaseWriter = supabaseWriter;
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mqttConfig = _config.GetSection("Mqtt");
        var host = mqttConfig["Host"]!;
        var port = int.Parse(mqttConfig["Port"] ?? "8883");
        var username = mqttConfig["Username"]!;
        var password = mqttConfig["Password"]!;
        var clientId = mqttConfig["ClientId"] ?? $"aquasense-backend-{Guid.NewGuid():N}";
        var topic = mqttConfig["Topic"] ?? "aquasense/+/sensors";

        var factory = new MqttFactory();
        using var client = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(host, port)
            .WithCredentials(username, password)
            .WithTlsOptions(o => o.UseTls())
            .WithCleanSession()
            .Build();

        client.ApplicationMessageReceivedAsync += async e =>
        {
            var payload = e.ApplicationMessage.ConvertPayloadToString();
            _logger.LogDebug("MQTT message received on {Topic}: {Payload}", e.ApplicationMessage.Topic, payload);

            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(payload, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data is null)
            {
                _logger.LogWarning("Failed to deserialize message: {Payload}", payload);
                return;
            }

            var reading = new SensorReading
            {
                DeviceId = data.TryGetValue("deviceId", out var deviceId) ? deviceId.ToString() : string.Empty,
                Timestamp = data.TryGetValue("timestamp", out var timestamp) && DateTime.TryParse(timestamp.ToString(), out var parsedTimestamp) ? parsedTimestamp : DateTime.UtcNow,
                Readings = data
            };

            await _supabaseWriter.WriteAsync(reading);
            await _hubContext.Clients.All.SendAsync("SensorUpdate", reading, stoppingToken);
        };

        client.DisconnectedAsync += async e =>
        {
            if (stoppingToken.IsCancellationRequested) return;
            _logger.LogWarning("MQTT disconnected. Reconnecting in 5s...");
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            try
            {
                await client.ConnectAsync(options, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT reconnect failed");
            }
        };

        _logger.LogInformation("Connecting to MQTT broker {Host}:{Port}", host, port);
        await client.ConnectAsync(options, stoppingToken);
        await client.SubscribeAsync(topic, cancellationToken: stoppingToken);
        _logger.LogInformation("Subscribed to topic {Topic}", topic);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
