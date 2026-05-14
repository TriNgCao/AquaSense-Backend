using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AquaSense.Backend.Repositories.Implementations;

public class DeviceRepository : BaseRepository, IDeviceRepository
{
    public DeviceRepository(IConfiguration config, ILogger<DeviceRepository> logger) : base(config, logger)
    {
    }

    public async Task<Device?> GetByIdAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT device_id, pond_id, device_code, device_name, installed_depth, firmware_version, last_seen, is_active, created_at
            FROM devices
            WHERE device_id = $1
            """);
        cmd.Parameters.AddWithValue(deviceId);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new Device
        {
            DeviceId = reader.GetString(0),
            PondId = reader.GetGuid(1),
            DeviceCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            DeviceName = reader.IsDBNull(3) ? null : reader.GetString(3),
            InstalledDepth = reader.IsDBNull(4) ? null : reader.GetDouble(4),
            FirmwareVersion = reader.IsDBNull(5) ? null : reader.GetString(5),
            LastSeen = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
            IsActive = reader.GetBoolean(7),
            CreatedAt = reader.GetDateTime(8)
        };
    }

    public async Task<IReadOnlyList<Device>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT device_id, pond_id, device_code, device_name, installed_depth, firmware_version, last_seen, is_active, created_at
            FROM devices
            ORDER BY created_at DESC
            """);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var results = new List<Device>();
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new Device
            {
                DeviceId = reader.GetString(0),
                PondId = reader.GetGuid(1),
                DeviceCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                DeviceName = reader.IsDBNull(3) ? null : reader.GetString(3),
                InstalledDepth = reader.IsDBNull(4) ? null : reader.GetDouble(4),
                FirmwareVersion = reader.IsDBNull(5) ? null : reader.GetString(5),
                LastSeen = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                IsActive = reader.GetBoolean(7),
                CreatedAt = reader.GetDateTime(8)
            });
        }

        return results;
    }

    public async Task CreateAsync(Device device, CancellationToken cancellationToken = default)
    {
        var createdAt = device.CreatedAt == default ? DateTime.UtcNow : device.CreatedAt;

        await using var cmd = CreateCommand(
            """
            INSERT INTO devices (device_id, pond_id, device_code, device_name, installed_depth, firmware_version, last_seen, is_active, created_at)
            VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)
            """);
        cmd.Parameters.AddWithValue(device.DeviceId);
        cmd.Parameters.AddWithValue(device.PondId);
        cmd.Parameters.AddWithValue(device.DeviceCode);
        cmd.Parameters.AddWithValue((object?)device.DeviceName ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)device.InstalledDepth ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)device.FirmwareVersion ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)device.LastSeen ?? DBNull.Value);
        cmd.Parameters.AddWithValue(device.IsActive);
        cmd.Parameters.AddWithValue(createdAt);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        device.CreatedAt = createdAt;
    }

    public async Task UpdateAsync(Device device, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            UPDATE devices
            SET pond_id = $2,
                device_code = $3,
                device_name = $4,
                installed_depth = $5,
                firmware_version = $6,
                last_seen = $7,
                is_active = $8
            WHERE device_id = $1
            """);
        cmd.Parameters.AddWithValue(device.DeviceId);
        cmd.Parameters.AddWithValue(device.PondId);
        cmd.Parameters.AddWithValue(device.DeviceCode);
        cmd.Parameters.AddWithValue((object?)device.DeviceName ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)device.InstalledDepth ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)device.FirmwareVersion ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)device.LastSeen ?? DBNull.Value);
        cmd.Parameters.AddWithValue(device.IsActive);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand("DELETE FROM devices WHERE device_id = $1");
        cmd.Parameters.AddWithValue(deviceId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
