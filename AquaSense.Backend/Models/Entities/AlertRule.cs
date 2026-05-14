using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSense.Backend.Models.Entities;

public class AlertRule
{
    [Key]
    public Guid RuleId { get; set; }

    [ForeignKey(nameof(Device))]
    [Required]
    public string DeviceId { get; set; } = string.Empty;
    public Device? Device { get; set; }

    [Required]
    public string Parameter { get; set; } = string.Empty;
    public double? MinThreshold { get; set; }
    public double? MaxThreshold { get; set; }
    [Required]
    public string Severity { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
