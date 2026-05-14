namespace AquaSense.Backend.Models.Request;

public class RegisterRequest
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? FullName { get; set; }
}
