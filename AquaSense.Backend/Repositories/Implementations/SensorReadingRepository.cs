using System.Text.Json;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;

namespace AquaSense.Backend.Repositories.Implementations;

public class SensorReadingRepository : BaseRepository, ISensorReadingRepository
{
    public SensorReadingRepository(IConfiguration config, ILogger<SensorReadingRepository> logger) : base(config, logger)
    {
    }

    public async Task<SensorReading?> GetByIdAsync(Guid readingId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT reading_id, device_id, readings, recorded_at
            FROM sensor_readings
            WHERE reading_id = $1
            """);
        cmd.Parameters.AddWithValue(readingId);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        var readingsJson = reader.GetFieldValue<string>(2);
        var readings = JsonSerializer.Deserialize<Dictionary<string, object>>(readingsJson) ?? new Dictionary<string, object>();

        return new SensorReading
        {
            ReadingId = reader.GetGuid(0),
            DeviceId = reader.GetString(1),
            Readings = readings,
            Timestamp = reader.GetDateTime(3)
        };
    }

    public async Task<IReadOnlyList<SensorReading>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT reading_id, device_id, readings, recorded_at
            FROM sensor_readings
            ORDER BY recorded_at DESC
            """);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var results = new List<SensorReading>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var readingsJson = reader.GetFieldValue<string>(2);
            var readings = JsonSerializer.Deserialize<Dictionary<string, object>>(readingsJson) ?? new Dictionary<string, object>();

            results.Add(new SensorReading
            {
                ReadingId = reader.GetGuid(0),
                DeviceId = reader.GetString(1),
                Readings = readings,
                Timestamp = reader.GetDateTime(3)
            });
        }

        return results;
    }

    public async Task CreateAsync(SensorReading reading, CancellationToken cancellationToken = default)
    {
        var readingId = reading.ReadingId == Guid.Empty ? Guid.NewGuid() : reading.ReadingId;
        var timestamp = reading.Timestamp == default ? DateTime.UtcNow : reading.Timestamp;

        await using var cmd = CreateCommand(
            """
            INSERT INTO sensor_readings (reading_id, device_id, readings, recorded_at)
            VALUES ($1, $2, $3, $4)
            """);
        cmd.Parameters.AddWithValue(readingId);
        cmd.Parameters.AddWithValue(reading.DeviceId);
        cmd.Parameters.AddWithValue(NpgsqlDbType.Jsonb, reading.Readings);
        cmd.Parameters.AddWithValue(timestamp);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        reading.ReadingId = readingId;
        reading.Timestamp = timestamp;
    }

    public async Task DeleteAsync(Guid readingId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand("DELETE FROM sensor_readings WHERE reading_id = $1");
        cmd.Parameters.AddWithValue(readingId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
