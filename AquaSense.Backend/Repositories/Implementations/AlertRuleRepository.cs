using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AquaSense.Backend.Repositories.Implementations;

public class AlertRuleRepository : BaseRepository, IAlertRuleRepository
{
    public AlertRuleRepository(IConfiguration config, ILogger<AlertRuleRepository> logger) : base(config, logger)
    {
    }

    public async Task<AlertRule?> GetByIdAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT rule_id, device_id, parameter, min_threshold, max_threshold, severity, message, is_active
            FROM alert_rules
            WHERE rule_id = $1
            """);
        cmd.Parameters.AddWithValue(ruleId);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new AlertRule
        {
            RuleId = reader.GetGuid(0),
            DeviceId = reader.GetString(1),
            Parameter = reader.GetString(2),
            MinThreshold = reader.IsDBNull(3) ? null : reader.GetDouble(3),
            MaxThreshold = reader.IsDBNull(4) ? null : reader.GetDouble(4),
            Severity = reader.GetString(5),
            Message = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            IsActive = reader.GetBoolean(7)
        };
    }

    public async Task<IReadOnlyList<AlertRule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT rule_id, device_id, parameter, min_threshold, max_threshold, severity, message, is_active
            FROM alert_rules
            ORDER BY rule_id DESC
            """);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var results = new List<AlertRule>();
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new AlertRule
            {
                RuleId = reader.GetGuid(0),
                DeviceId = reader.GetString(1),
                Parameter = reader.GetString(2),
                MinThreshold = reader.IsDBNull(3) ? null : reader.GetDouble(3),
                MaxThreshold = reader.IsDBNull(4) ? null : reader.GetDouble(4),
                Severity = reader.GetString(5),
                Message = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                IsActive = reader.GetBoolean(7)
            });
        }

        return results;
    }

    public async Task CreateAsync(AlertRule rule, CancellationToken cancellationToken = default)
    {
        var ruleId = rule.RuleId == Guid.Empty ? Guid.NewGuid() : rule.RuleId;

        await using var cmd = CreateCommand(
            """
            INSERT INTO alert_rules (rule_id, device_id, parameter, min_threshold, max_threshold, severity, message, is_active)
            VALUES ($1, $2, $3, $4, $5, $6, $7, $8)
            """);
        cmd.Parameters.AddWithValue(ruleId);
        cmd.Parameters.AddWithValue(rule.DeviceId);
        cmd.Parameters.AddWithValue(rule.Parameter);
        cmd.Parameters.AddWithValue((object?)rule.MinThreshold ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)rule.MaxThreshold ?? DBNull.Value);
        cmd.Parameters.AddWithValue(rule.Severity);
        cmd.Parameters.AddWithValue((object?)rule.Message ?? string.Empty);
        cmd.Parameters.AddWithValue(rule.IsActive);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        rule.RuleId = ruleId;
    }

    public async Task UpdateAsync(AlertRule rule, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            UPDATE alert_rules
            SET device_id = $2,
                parameter = $3,
                min_threshold = $4,
                max_threshold = $5,
                severity = $6,
                message = $7,
                is_active = $8
            WHERE rule_id = $1
            """);
        cmd.Parameters.AddWithValue(rule.RuleId);
        cmd.Parameters.AddWithValue(rule.DeviceId);
        cmd.Parameters.AddWithValue(rule.Parameter);
        cmd.Parameters.AddWithValue((object?)rule.MinThreshold ?? DBNull.Value);
        cmd.Parameters.AddWithValue((object?)rule.MaxThreshold ?? DBNull.Value);
        cmd.Parameters.AddWithValue(rule.Severity);
        cmd.Parameters.AddWithValue((object?)rule.Message ?? string.Empty);
        cmd.Parameters.AddWithValue(rule.IsActive);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand("DELETE FROM alert_rules WHERE rule_id = $1");
        cmd.Parameters.AddWithValue(ruleId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
