using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;

namespace AquaSense.Backend.Services.Interfaces;

public interface IAlertService
{
    Task<AlertDto?> GetByIdAsync(Guid alertId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AlertDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AlertDto> CreateAsync(AlertRequest request, CancellationToken cancellationToken = default);
    Task<AlertDto> UpdateAsync(Guid alertId, AlertRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid alertId, CancellationToken cancellationToken = default);
}
