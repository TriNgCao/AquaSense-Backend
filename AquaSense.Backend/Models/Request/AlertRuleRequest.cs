namespace AquaSense.Backend.Models.Request;

public class AlertRuleRequest
{
    public string DeviceId { get; set; } = string.Empty;
    public string Parameter { get; set; } = string.Empty;
    public double? MinThreshold { get; set; }
    public double? MaxThreshold { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
