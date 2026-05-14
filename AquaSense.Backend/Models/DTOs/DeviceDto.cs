namespace AquaSense.Backend.Models.DTOs;

public class DeviceDto
{
    public string DeviceId { get; set; } = string.Empty;
    public Guid PondId { get; set; }
    public string DeviceCode { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public double? InstalledDepth { get; set; }
    public string? FirmwareVersion { get; set; }
    public DateTime? LastSeen { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
