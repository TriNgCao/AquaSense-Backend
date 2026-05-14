namespace AquaSense.Backend.Models.DTOs;

public class AlertRuleDto
{
    public Guid RuleId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string Parameter { get; set; } = string.Empty;
    public double? MinThreshold { get; set; }
    public double? MaxThreshold { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
