using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Models.Mappings;

public static class AlertRuleMapping
{
    public static AlertRuleDto ToDto(this AlertRule entity)
    {
        return new AlertRuleDto
        {
            RuleId = entity.RuleId,
            DeviceId = entity.DeviceId,
            Parameter = entity.Parameter,
            MinThreshold = entity.MinThreshold,
            MaxThreshold = entity.MaxThreshold,
            Severity = entity.Severity,
            Message = entity.Message,
            IsActive = entity.IsActive
        };
    }

    public static AlertRule ToEntity(this AlertRuleDto dto)
    {
        return new AlertRule
        {
            RuleId = dto.RuleId,
            DeviceId = dto.DeviceId,
            Parameter = dto.Parameter,
            MinThreshold = dto.MinThreshold,
            MaxThreshold = dto.MaxThreshold,
            Severity = dto.Severity,
            Message = dto.Message,
            IsActive = dto.IsActive
        };
    }
}
