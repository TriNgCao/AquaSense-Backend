using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSense.Backend.Models.Entities;

public class Device
{
    [Key]
    public string DeviceId { get; set; } = string.Empty;

    [ForeignKey(nameof(Pond))]
    [Required]
    public Guid PondId { get; set; }
    public Pond? Pond { get; set; }

    [Required]
    public string DeviceCode { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public double? InstalledDepth { get; set; }
    public string? FirmwareVersion { get; set; }
    public DateTime? LastSeen { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
