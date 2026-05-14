using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Models.Mappings;

public static class AlertMapping
{
    public static AlertDto ToDto(this Alert entity)
    {
        return new AlertDto
        {
            AlertId = entity.AlertId,
            ReadingId = entity.ReadingId,
            RuleId = entity.RuleId,
            TriggeredAt = entity.TriggeredAt,
            IsResolved = entity.IsResolved,
            ResolvedAt = entity.ResolvedAt,
            ResolvedBy = entity.ResolvedBy
        };
    }

    public static Alert ToEntity(this AlertDto dto)
    {
        return new Alert
        {
            AlertId = dto.AlertId,
            ReadingId = dto.ReadingId,
            RuleId = dto.RuleId,
            TriggeredAt = dto.TriggeredAt,
            IsResolved = dto.IsResolved,
            ResolvedAt = dto.ResolvedAt,
            ResolvedBy = dto.ResolvedBy
        };
    }
}
