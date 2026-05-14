namespace AquaSense.Backend.Models.Request;

public class SensorReadingRequest
{
    public string DeviceId { get; set; } = string.Empty;
    public Dictionary<string, object> Readings { get; set; } = new();
    public DateTime? Timestamp { get; set; }
}
