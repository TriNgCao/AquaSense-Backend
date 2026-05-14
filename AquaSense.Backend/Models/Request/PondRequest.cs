namespace AquaSense.Backend.Models.Request;

public class PondRequest
{
    public Guid UserId { get; set; }
    public string PondName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Area { get; set; }
    public double? DepthAvg { get; set; }
    public double? StockingDensity { get; set; }
}
