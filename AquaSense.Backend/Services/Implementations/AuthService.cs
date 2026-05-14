using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AquaSense.Backend.Services.Implementations;

public class AuthService : IAuthService
{
    private const int Pbkdf2Iterations = 100_000;
    private const int SaltSize = 16;
    private const int KeySize = 32;

    private readonly IUserRepository _users;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository users, IConfiguration config)
    {
        _users = users;
        _config = config;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingByEmail = await _users.GetByEmailAsync(request.Email, cancellationToken);
            if (existingByEmail is not null)
                throw new InvalidOperationException("Email already registered");
        }

        var existingByPhone = await _users.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);
        if (existingByPhone is not null)
            throw new InvalidOperationException("Phone number already registered");

        var username = string.IsNullOrWhiteSpace(request.Username)
            ? request.PhoneNumber
            : request.Username;

        var entity = new User
        {
            PhoneNumber = request.PhoneNumber,
            Username = username,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email,
            PasswordHash = HashPassword(request.Password),
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow
        };

        await _users.CreateAsync(entity, cancellationToken);
        return BuildAuthResponse(entity);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        User? user = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
            user = await _users.GetByEmailAsync(request.Email, cancellationToken);
        else if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            user = await _users.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

        if (user is null || !VerifyPassword(request.Password, user.PasswordHash))
            return null;

        return BuildAuthResponse(user);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var (token, expiresAt) = GenerateToken(user);
        return new AuthResponse
        {
            User = user.ToDto(),
            Token = token,
            ExpiresAt = expiresAt
        };
    }

    private (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing");
        var audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing");
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
        var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var minutes) ? minutes : 60*24;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username),
            new("phone_number", user.PhoneNumber)
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Pbkdf2Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return $"pbkdf2${Pbkdf2Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string stored)
    {
        var parts = stored.Split('$', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 4 || parts[0] != "pbkdf2")
            return false;

        if (!int.TryParse(parts[1], out var iterations))
            return false;

        var salt = Convert.FromBase64String(parts[2]);
        var expectedHash = Convert.FromBase64String(parts[3]);

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
