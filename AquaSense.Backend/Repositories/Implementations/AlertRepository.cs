using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AquaSense.Backend.Repositories;

namespace AquaSense.Backend.Repositories.Implementations;

public class AlertRepository : BaseRepository, IAlertRepository
{
    public AlertRepository(IConfiguration config, ILogger<AlertRepository> logger) : base(config, logger)
    {
    }

    public async Task<Alert?> GetByIdAsync(Guid alertId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT alert_id, reading_id, rule_id, triggered_at, is_resolved, resolved_at, resolved_by
            FROM alerts
            WHERE alert_id = $1
            """);
        cmd.Parameters.AddWithValue(alertId);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new Alert
        {
            AlertId = reader.GetGuid(0),
            ReadingId = reader.GetGuid(1),
            RuleId = reader.GetGuid(2),
            TriggeredAt = reader.GetDateTime(3),
            IsResolved = reader.GetBoolean(4),
            ResolvedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
            ResolvedBy = reader.IsDBNull(6) ? null : reader.GetGuid(6)
        };
    }

    public async Task<IReadOnlyList<Alert>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT alert_id, reading_id, rule_id, triggered_at, is_resolved, resolved_at, resolved_by
            FROM alerts
            ORDER BY triggered_at DESC
            """);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var results = new List<Alert>();
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new Alert
            {
                AlertId = reader.GetGuid(0),
                ReadingId = reader.GetGuid(1),
                RuleId = reader.GetGuid(2),
                TriggeredAt = reader.GetDateTime(3),
                IsResolved = reader.GetBoolean(4),
                ResolvedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                ResolvedBy = reader.IsDBNull(6) ? null : reader.GetGuid(6)
            });
        }

        return results;
    }

    public async Task CreateAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        var alertId = alert.AlertId == Guid.Empty ? Guid.NewGuid() : alert.AlertId;
        var triggeredAt = alert.TriggeredAt == default ? DateTime.UtcNow : alert.TriggeredAt;

        await using var cmd = CreateCommand(
            """
            INSERT INTO alerts (alert_id, reading_id, rule_id, triggered_at, is_resolved, resolved_at, resolved_by)
            VALUES ($1, $2, $3, $4, $5, $6, $7)
            """);
        cmd.Parameters.AddWithValue(alertId);
        cmd.Parameters.AddWithValue(alert.ReadingId);
        cmd.Parameters.AddWithValue(alert.RuleId);
        cmd.Parameters.AddWithValue(triggeredAt);
        cmd.Parameters.AddWithValue(alert.IsResolved);
        cmd.Parameters.AddWithValue((object?)alert.ResolvedAt ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)alert.ResolvedBy ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        alert.AlertId = alertId;
        alert.TriggeredAt = triggeredAt;
    }

    public async Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            UPDATE alerts
            SET reading_id = $2,
                rule_id = $3,
                triggered_at = $4,
                is_resolved = $5,
                resolved_at = $6,
                resolved_by = $7
            WHERE alert_id = $1
            """);
        cmd.Parameters.AddWithValue(alert.AlertId);
        cmd.Parameters.AddWithValue(alert.ReadingId);
        cmd.Parameters.AddWithValue(alert.RuleId);
        cmd.Parameters.AddWithValue(alert.TriggeredAt);
        cmd.Parameters.AddWithValue(alert.IsResolved);
        cmd.Parameters.AddWithValue((object?)alert.ResolvedAt ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)alert.ResolvedBy ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid alertId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand("DELETE FROM alerts WHERE alert_id = $1");
        cmd.Parameters.AddWithValue(alertId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
