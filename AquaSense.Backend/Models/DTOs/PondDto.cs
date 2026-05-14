namespace AquaSense.Backend.Models.DTOs;

public class PondDto
{
    public Guid PondId { get; set; }
    public Guid UserId { get; set; }
    public string PondName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Area { get; set; }
    public double? DepthAvg { get; set; }
    public double? StockingDensity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
