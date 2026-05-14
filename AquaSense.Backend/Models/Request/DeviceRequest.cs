namespace AquaSense.Backend.Models.Request;

public class DeviceRequest
{
    public Guid PondId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceCode { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public double? InstalledDepth { get; set; }
    public string? FirmwareVersion { get; set; }
    public DateTime? LastSeen { get; set; }
    public bool IsActive { get; set; } = true;
}
