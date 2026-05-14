using AquaSense.Backend.Models.Entities;
using Npgsql;
using System.Text.Json;

namespace AquaSense.Backend.Services;

public class SupabaseWriter
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly ILogger<SupabaseWriter> _logger;

    public SupabaseWriter(IConfiguration config, ILogger<SupabaseWriter> logger)
    {
        _logger = logger;
        var connectionString = config.GetConnectionString("Supabase")
            ?? throw new InvalidOperationException("Connection string 'Supabase' is missing.");

        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public async Task WriteAsync(SensorReading reading)
    {
        try
        {
            await using var cmd = _dataSource.CreateCommand(
                """
                INSERT INTO sensor_readings (device_id, readings, recorded_at)
                VALUES ($1, $2::jsonb, $3)
                """);

            var json = JsonSerializer.Serialize(reading.Readings);

            cmd.Parameters.AddWithValue(reading.DeviceId);
            cmd.Parameters.AddWithValue(json);
            cmd.Parameters.AddWithValue(reading.Timestamp);

            await cmd.ExecuteNonQueryAsync();
            _logger.LogDebug("Wrote reading for device {DeviceId} to Supabase", reading.DeviceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write reading for device {DeviceId} to Supabase", reading.DeviceId);
        }
    }
}
