namespace AquaSense.Backend.Models.DTOs;

public class SensorReadingDto
{
    public Guid ReadingId { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public Dictionary<string, object> Readings { get; set; } = new();
    public DateTime Timestamp { get; set; }
}
