using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;

namespace AquaSense.Backend.Services.Implementations;

public class AlertRuleService : IAlertRuleService
{
    private readonly IAlertRuleRepository _repository;

    public AlertRuleService(IAlertRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<AlertRuleDto?> GetByIdAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        var rule = await _repository.GetByIdAsync(ruleId, cancellationToken);
        return rule?.ToDto();
    }

    public async Task<IReadOnlyList<AlertRuleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rules = await _repository.GetAllAsync(cancellationToken);
        return rules.Select(r => r.ToDto()).ToList();
    }

    public async Task<AlertRuleDto> CreateAsync(AlertRuleRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new AlertRule
        {
            DeviceId = request.DeviceId,
            Parameter = request.Parameter,
            MinThreshold = request.MinThreshold,
            MaxThreshold = request.MaxThreshold,
            Severity = request.Severity,
            Message = request.Message,
            IsActive = request.IsActive
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public async Task<AlertRuleDto> UpdateAsync(Guid ruleId, AlertRuleRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new AlertRule
        {
            RuleId = ruleId,
            DeviceId = request.DeviceId,
            Parameter = request.Parameter,
            MinThreshold = request.MinThreshold,
            MaxThreshold = request.MaxThreshold,
            Severity = request.Severity,
            Message = request.Message,
            IsActive = request.IsActive
        };

        await _repository.UpdateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public Task DeleteAsync(Guid ruleId, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(ruleId, cancellationToken);
    }
}
