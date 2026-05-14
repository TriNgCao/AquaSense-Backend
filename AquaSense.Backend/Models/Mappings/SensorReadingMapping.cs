using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Models.Mappings;

public static class SensorReadingMapping
{
    public static SensorReadingDto ToDto(this SensorReading entity)
    {
        return new SensorReadingDto
        {
            ReadingId = entity.ReadingId,
            DeviceId = entity.DeviceId,
            Readings = entity.Readings,
            Timestamp = entity.Timestamp
        };
    }

    public static SensorReading ToEntity(this SensorReadingDto dto)
    {
        return new SensorReading
        {
            ReadingId = dto.ReadingId,
            DeviceId = dto.DeviceId,
            Readings = dto.Readings,
            Timestamp = dto.Timestamp
        };
    }
}
