namespace AquaSense.Backend.Models.DTOs;

public class UserDto
{
    public Guid UserId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; }
}
