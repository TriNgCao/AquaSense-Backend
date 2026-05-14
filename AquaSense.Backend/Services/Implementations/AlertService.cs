using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;

namespace AquaSense.Backend.Services.Implementations;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _repository;

    public AlertService(IAlertRepository repository)
    {
        _repository = repository;
    }

    public async Task<AlertDto?> GetByIdAsync(Guid alertId, CancellationToken cancellationToken = default)
    {
        var alert = await _repository.GetByIdAsync(alertId, cancellationToken);
        return alert?.ToDto();
    }

    public async Task<IReadOnlyList<AlertDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var alerts = await _repository.GetAllAsync(cancellationToken);
        return alerts.Select(a => a.ToDto()).ToList();
    }

    public async Task<AlertDto> CreateAsync(AlertRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Alert
        {
            ReadingId = request.ReadingId,
            RuleId = request.RuleId,
            TriggeredAt = request.TriggeredAt ?? DateTime.UtcNow,
            IsResolved = request.IsResolved,
            ResolvedAt = request.ResolvedAt,
            ResolvedBy = request.ResolvedBy
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public async Task<AlertDto> UpdateAsync(Guid alertId, AlertRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new Alert
        {
            AlertId = alertId,
            ReadingId = request.ReadingId,
            RuleId = request.RuleId,
            TriggeredAt = request.TriggeredAt ?? DateTime.UtcNow,
            IsResolved = request.IsResolved,
            ResolvedAt = request.ResolvedAt,
            ResolvedBy = request.ResolvedBy
        };

        await _repository.UpdateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public Task DeleteAsync(Guid alertId, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(alertId, cancellationToken);
    }
}
