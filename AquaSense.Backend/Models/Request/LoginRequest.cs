namespace AquaSense.Backend.Models.Request;

public class LoginRequest
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = string.Empty;
}
