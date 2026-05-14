using AquaSense.Backend.Models.DTOs;

namespace AquaSense.Backend.Models.Response;

public class AuthResponse
{
    public UserDto User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
