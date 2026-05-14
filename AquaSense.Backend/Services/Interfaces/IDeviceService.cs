using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;

namespace AquaSense.Backend.Services.Interfaces;

public interface IDeviceService
{
    Task<DeviceDto?> GetByIdAsync(string deviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeviceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DeviceDto> CreateAsync(DeviceRequest request, CancellationToken cancellationToken = default);
    Task<DeviceDto> UpdateAsync(string deviceId, DeviceRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(string deviceId, CancellationToken cancellationToken = default);
}
