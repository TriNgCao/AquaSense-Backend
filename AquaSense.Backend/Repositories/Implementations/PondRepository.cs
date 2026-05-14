using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AquaSense.Backend.Repositories.Implementations;

public class PondRepository : BaseRepository, IPondRepository
{
    public PondRepository(IConfiguration config, ILogger<PondRepository> logger) : base(config, logger)
    {
    }

    public async Task<Pond?> GetByIdAsync(Guid pondId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT pond_id, user_id, pond_name, location, area, depth_avg, stocking_density, created_at, updated_at
            FROM ponds
            WHERE pond_id = $1
            """);
        cmd.Parameters.AddWithValue(pondId);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new Pond
        {
            PondId = reader.GetGuid(0),
            UserId = reader.GetGuid(1),
            PondName = reader.GetString(2),
            Location = reader.IsDBNull(3) ? null : reader.GetString(3),
            Area = reader.IsDBNull(4) ? null : reader.GetDouble(4),
            DepthAvg = reader.IsDBNull(5) ? null : reader.GetDouble(5),
            StockingDensity = reader.IsDBNull(6) ? null : reader.GetDouble(6),
            CreatedAt = reader.GetDateTime(7),
            UpdatedAt = reader.GetDateTime(8)
        };
    }

    public async Task<IReadOnlyList<Pond>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT pond_id, user_id, pond_name, location, area, depth_avg, stocking_density, created_at, updated_at
            FROM ponds
            ORDER BY created_at DESC
            """);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var results = new List<Pond>();
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new Pond
            {
                PondId = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                PondName = reader.GetString(2),
                Location = reader.IsDBNull(3) ? null : reader.GetString(3),
                Area = reader.IsDBNull(4) ? null : reader.GetDouble(4),
                DepthAvg = reader.IsDBNull(5) ? null : reader.GetDouble(5),
                StockingDensity = reader.IsDBNull(6) ? null : reader.GetDouble(6),
                CreatedAt = reader.GetDateTime(7),
                UpdatedAt = reader.GetDateTime(8)
            });
        }

        return results;
    }

    public async Task CreateAsync(Pond pond, CancellationToken cancellationToken = default)
    {
        var pondId = pond.PondId == Guid.Empty ? Guid.NewGuid() : pond.PondId;
        var createdAt = pond.CreatedAt == default ? DateTime.UtcNow : pond.CreatedAt;
        var updatedAt = pond.UpdatedAt == default ? DateTime.UtcNow : pond.UpdatedAt;

        await using var cmd = CreateCommand(
            """
            INSERT INTO ponds (pond_id, user_id, pond_name, location, area, depth_avg, stocking_density, created_at, updated_at)
            VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)
            """);
        cmd.Parameters.AddWithValue(pondId);
        cmd.Parameters.AddWithValue(pond.UserId);
        cmd.Parameters.AddWithValue(pond.PondName);
        cmd.Parameters.AddWithValue((object?)pond.Location ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)pond.Area ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)pond.DepthAvg ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)pond.StockingDensity ?? DBNull.Value);
        cmd.Parameters.AddWithValue(createdAt);
        cmd.Parameters.AddWithValue(updatedAt);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        pond.PondId = pondId;
        pond.CreatedAt = createdAt;
        pond.UpdatedAt = updatedAt;
    }

    public async Task UpdateAsync(Pond pond, CancellationToken cancellationToken = default)
    {
        var updatedAt = DateTime.UtcNow;

        await using var cmd = CreateCommand(
            """
            UPDATE ponds
            SET user_id = $2,
                pond_name = $3,
                location = $4,
                area = $5,
                depth_avg = $6,
                stocking_density = $7,
                updated_at = $8
            WHERE pond_id = $1
            """);
        cmd.Parameters.AddWithValue(pond.PondId);
        cmd.Parameters.AddWithValue(pond.UserId);
        cmd.Parameters.AddWithValue(pond.PondName);
        cmd.Parameters.AddWithValue((object?)pond.Location ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)pond.Area ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)pond.DepthAvg ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)pond.StockingDensity ?? DBNull.Value);
        cmd.Parameters.AddWithValue(updatedAt);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        pond.UpdatedAt = updatedAt;
    }

    public async Task DeleteAsync(Guid pondId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand("DELETE FROM ponds WHERE pond_id = $1");
        cmd.Parameters.AddWithValue(pondId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
