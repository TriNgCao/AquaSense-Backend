using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Repositories.Interfaces;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(Guid alertId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Alert>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(Alert alert, CancellationToken cancellationToken = default);
    Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid alertId, CancellationToken cancellationToken = default);
}
