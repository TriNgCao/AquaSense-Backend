using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Repositories.Interfaces;

public interface IPondRepository
{
    Task<Pond?> GetByIdAsync(Guid pondId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Pond>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(Pond pond, CancellationToken cancellationToken = default);
    Task UpdateAsync(Pond pond, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid pondId, CancellationToken cancellationToken = default);
}
