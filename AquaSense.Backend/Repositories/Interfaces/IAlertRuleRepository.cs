using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Repositories.Interfaces;

public interface IAlertRuleRepository
{
    Task<AlertRule?> GetByIdAsync(Guid ruleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AlertRule>> GetAllAsync(CancellationToken cancellationToken = default);
    Task CreateAsync(AlertRule rule, CancellationToken cancellationToken = default);
    Task UpdateAsync(AlertRule rule, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid ruleId, CancellationToken cancellationToken = default);
}
