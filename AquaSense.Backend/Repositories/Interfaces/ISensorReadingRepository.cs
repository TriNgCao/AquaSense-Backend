using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Repositories.Interfaces;

public interface ISensorReadingRepository
{
    Task<SensorReading?> GetByIdAsync(Guid readingId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SensorReading>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(SensorReading reading, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid readingId, CancellationToken cancellationToken = default);
}
