using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;

namespace AquaSense.Backend.Services.Implementations;

public class PondService : IPondService
{
    private readonly IPondRepository _repository;

    public PondService(IPondRepository repository)
    {
        _repository = repository;
    }

    public async Task<PondDto?> GetByIdAsync(Guid pondId, CancellationToken cancellationToken = default)
    {
        var pond = await _repository.GetByIdAsync(pondId, cancellationToken);
        return pond?.ToDto();
    }

    public async Task<IReadOnlyList<PondDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var ponds = await _repository.GetAllAsync(cancellationToken);
        return ponds.Select(p => p.ToDto()).ToList();
    }

    public async Task<PondDto> CreateAsync(PondRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Pond
        {
            UserId = request.UserId,
            PondName = request.PondName,
            Location = request.Location,
            Area = request.Area,
            DepthAvg = request.DepthAvg,
            StockingDensity = request.StockingDensity
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public async Task<PondDto> UpdateAsync(Guid pondId, PondRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Pond
        {
            PondId = pondId,
            UserId = request.UserId,
            PondName = request.PondName,
            Location = request.Location,
            Area = request.Area,
            DepthAvg = request.DepthAvg,
            StockingDensity = request.StockingDensity
        };

        await _repository.UpdateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public Task DeleteAsync(Guid pondId, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(pondId, cancellationToken);
    }
}
