using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;

namespace AquaSense.Backend.Services.Implementations;

public class SensorReadingService : ISensorReadingService
{
    private readonly ISensorReadingRepository _repository;

    public SensorReadingService(ISensorReadingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SensorReadingDto?> GetByIdAsync(Guid readingId, CancellationToken cancellationToken = default)
    {
        var reading = await _repository.GetByIdAsync(readingId, cancellationToken);
        return reading?.ToDto();
    }

    public async Task<IReadOnlyList<SensorReadingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var readings = await _repository.GetAllAsync(cancellationToken);
        return readings.Select(r => r.ToDto()).ToList();
    }

    public async Task<SensorReadingDto> CreateAsync(SensorReadingRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new SensorReading
        {
            DeviceId = request.DeviceId,
            Readings = request.Readings,
            Timestamp = request.Timestamp ?? DateTime.UtcNow
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public Task DeleteAsync(Guid readingId, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(readingId, cancellationToken);
    }
}
