using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSense.Backend.Models.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(20)]
    [RegularExpression("^\\+?[1-9]\\d{7,14}$")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FullName { get; set; }

    public DateTime CreatedAt { get; set; }
}