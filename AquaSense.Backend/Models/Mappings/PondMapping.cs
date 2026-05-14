using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Models.Mappings;

public static class PondMapping
{
    public static PondDto ToDto(this Pond entity)
    {
        return new PondDto
        {
            PondId = entity.PondId,
            UserId = entity.UserId,
            PondName = entity.PondName,
            Location = entity.Location,
            Area = entity.Area,
            DepthAvg = entity.DepthAvg,
            StockingDensity = entity.StockingDensity,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public static Pond ToEntity(this PondDto dto)
    {
        return new Pond
        {
            PondId = dto.PondId,
            UserId = dto.UserId,
            PondName = dto.PondName,
            Location = dto.Location,
            Area = dto.Area,
            DepthAvg = dto.DepthAvg,
            StockingDensity = dto.StockingDensity,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
