using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSense.Backend.Models.Entities;

public class Pond
{
    [Key]
    public Guid PondId { get; set; }

    [ForeignKey(nameof(User))]
    [Required]
    public Guid UserId { get; set; }
    public User? User { get; set; }

    [Required]
    public string PondName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Area { get; set; }
    public double? DepthAvg { get; set; }
    public double? StockingDensity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
