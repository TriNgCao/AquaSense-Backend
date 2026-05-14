using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;

namespace AquaSense.Backend.Services.Implementations;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;

    public DeviceService(IDeviceRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeviceDto?> GetByIdAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        var device = await _repository.GetByIdAsync(deviceId, cancellationToken);
        return device?.ToDto();
    }

    public async Task<IReadOnlyList<DeviceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var devices = await _repository.GetAllAsync(cancellationToken);
        return devices.Select(d => d.ToDto()).ToList();
    }

    public async Task<DeviceDto> CreateAsync(DeviceRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Device
        {
            DeviceId = request.DeviceId,
            PondId = request.PondId,
            DeviceCode = request.DeviceCode,
            DeviceName = request.DeviceName,
            InstalledDepth = request.InstalledDepth,
            FirmwareVersion = request.FirmwareVersion,
            LastSeen = request.LastSeen,
            IsActive = request.IsActive
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public async Task<DeviceDto> UpdateAsync(string deviceId, DeviceRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Device
        {
            DeviceId = deviceId,
            PondId = request.PondId,
            DeviceCode = request.DeviceCode,
            DeviceName = request.DeviceName,
            InstalledDepth = request.InstalledDepth,
            FirmwareVersion = request.FirmwareVersion,
            LastSeen = request.LastSeen,
            IsActive = request.IsActive
        };

        await _repository.UpdateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public Task DeleteAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(deviceId, cancellationToken);
    }
}
