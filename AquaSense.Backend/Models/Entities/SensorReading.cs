using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSense.Backend.Models.Entities;

public class SensorReading
{
    [Key]
    public Guid ReadingId { get; set; }

    [ForeignKey(nameof(Device))]
    [Required]
    public string DeviceId { get; set; } = string.Empty;
    public Device? Device { get; set; }

    [Required]
    public Dictionary<string, object> Readings { get; set; } = new();
    public DateTime Timestamp { get; set; }
}
