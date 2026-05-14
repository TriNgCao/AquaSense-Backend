using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;

namespace AquaSense.Backend.Services.Interfaces;

public interface IPondService
{
    Task<PondDto?> GetByIdAsync(Guid pondId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PondDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PondDto> CreateAsync(PondRequest request, CancellationToken cancellationToken = default);
    Task<PondDto> UpdateAsync(Guid pondId, PondRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid pondId, CancellationToken cancellationToken = default);
}
