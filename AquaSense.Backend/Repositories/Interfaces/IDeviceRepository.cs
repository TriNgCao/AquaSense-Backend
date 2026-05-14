using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(string deviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Device>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(Device device, CancellationToken cancellationToken = default);
    Task UpdateAsync(Device device, CancellationToken cancellationToken = default);
    Task DeleteAsync(string deviceId, CancellationToken cancellationToken = default);
}
