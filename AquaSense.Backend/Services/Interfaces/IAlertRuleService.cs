using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;

namespace AquaSense.Backend.Services.Interfaces;

public interface IAlertRuleService
{
    Task<AlertRuleDto?> GetByIdAsync(Guid ruleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AlertRuleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AlertRuleDto> CreateAsync(AlertRuleRequest request, CancellationToken cancellationToken = default);
    Task<AlertRuleDto> UpdateAsync(Guid ruleId, AlertRuleRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid ruleId, CancellationToken cancellationToken = default);
}
