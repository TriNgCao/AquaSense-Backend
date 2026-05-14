using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

// ── Config ────────────────────────────────────────────────────────────────────
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Local.json", optional: true)   // local secrets, never committed
    .Build();

var host       = config["Mqtt:Host"] ?? throw new InvalidOperationException("Mqtt:Host is required");
var port       = int.Parse(config["Mqtt:Port"] ?? "8883");
var username   = config["Mqtt:Username"] ?? throw new InvalidOperationException("Mqtt:Username is required");
var password   = config["Mqtt:Password"] ?? throw new InvalidOperationException("Mqtt:Password is required");
var deviceId   = config["Simulator:DeviceId"] ?? "device-sim-01";
var intervalMs = int.Parse(config["Simulator:IntervalMs"] ?? "2000");
var topic      = $"aquasense/{deviceId}/sensors";

// ── Connect ───────────────────────────────────────────────────────────────────
var factory = new MqttFactory();
using var client = factory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
    .WithClientId($"aquasense-sim-{Guid.NewGuid():N}")
    .WithTcpServer(host, port)
    .WithCredentials(username, password)
    .WithTlsOptions(o => o.UseTls())
    .WithCleanSession()
    .Build();

Console.WriteLine($"Connecting to {host}:{port}...");
await client.ConnectAsync(options);
Console.WriteLine($"Connected. Publishing to topic: {topic}  (every {intervalMs}ms)");
Console.WriteLine("Press Ctrl+C to stop.\n");

// ── Simulate sensor data ──────────────────────────────────────────────────────
var rng = new Random();
double temperature     = 28.0;
double ph              = 7.2;
double dissolvedOxygen = 7.0;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

while (!cts.IsCancellationRequested)
{
    // Drift values slightly each tick to simulate real sensor noise
    temperature     += (rng.NextDouble() - 0.5) * 0.2;
    ph              += (rng.NextDouble() - 0.5) * 0.05;
    dissolvedOxygen += (rng.NextDouble() - 0.5) * 0.1;

    // Clamp to realistic aquaculture ranges
    temperature     = Math.Clamp(temperature,     20.0, 35.0);
    ph              = Math.Clamp(ph,               6.0,  9.0);
    dissolvedOxygen = Math.Clamp(dissolvedOxygen,  4.0, 12.0);

    var payload = JsonSerializer.Serialize(new
    {
        deviceId,
        temperature     = Math.Round(temperature,     2),
        ph              = Math.Round(ph,              2),
        dissolvedOxygen = Math.Round(dissolvedOxygen, 2),
        timestamp       = DateTime.UtcNow
    });

    var message = new MqttApplicationMessageBuilder()
        .WithTopic(topic)
        .WithPayload(Encoding.UTF8.GetBytes(payload))
        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
        .WithRetainFlag(false)
        .Build();

    await client.PublishAsync(message, cts.Token);
    Console.WriteLine($"[{DateTime.UtcNow:HH:mm:ss}] {payload}");

    try { await Task.Delay(intervalMs, cts.Token); }
    catch (TaskCanceledException) { break; }
}

Console.WriteLine("\nSimulator stopped.");
await client.DisconnectAsync();
