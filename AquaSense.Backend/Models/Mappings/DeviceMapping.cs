using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Models.Mappings;

public static class DeviceMapping
{
    public static DeviceDto ToDto(this Device entity)
    {
        return new DeviceDto
        {
            DeviceId = entity.DeviceId,
            PondId = entity.PondId,
            DeviceCode = entity.DeviceCode,
            DeviceName = entity.DeviceName,
            InstalledDepth = entity.InstalledDepth,
            FirmwareVersion = entity.FirmwareVersion,
            LastSeen = entity.LastSeen,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
    }

    public static Device ToEntity(this DeviceDto dto)
    {
        return new Device
        {
            DeviceId = dto.DeviceId,
            PondId = dto.PondId,
            DeviceCode = dto.DeviceCode,
            DeviceName = dto.DeviceName,
            InstalledDepth = dto.InstalledDepth,
            FirmwareVersion = dto.FirmwareVersion,
            LastSeen = dto.LastSeen,
            IsActive = dto.IsActive,
            CreatedAt = dto.CreatedAt
        };
    }
}
