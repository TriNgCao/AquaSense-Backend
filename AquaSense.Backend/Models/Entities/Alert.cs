using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSense.Backend.Models.Entities;

public class Alert
{
    [Key]
    public Guid AlertId { get; set; }

    [ForeignKey(nameof(SensorReading))]
    [Required]
    public Guid ReadingId { get; set; }
    public SensorReading? SensorReading { get; set; }

    [ForeignKey(nameof(AlertRule))]
    [Required]
    public Guid RuleId { get; set; }
    public AlertRule? AlertRule { get; set; }

    public DateTime TriggeredAt { get; set; }
    public bool IsResolved { get; set; } 
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedBy { get; set; } 
}
