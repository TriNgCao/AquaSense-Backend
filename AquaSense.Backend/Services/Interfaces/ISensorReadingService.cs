using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;

namespace AquaSense.Backend.Services.Interfaces;

public interface ISensorReadingService
{
    Task<SensorReadingDto?> GetByIdAsync(Guid readingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SensorReadingDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SensorReadingDto> CreateAsync(SensorReadingRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid readingId, CancellationToken cancellationToken = default);
}
