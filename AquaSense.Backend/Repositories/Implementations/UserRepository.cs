using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AquaSense.Backend.Repositories.Implementations;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration config, ILogger<UserRepository> logger) : base(config, logger)
    {
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT user_id, phone_number, username, email, password_hash, full_name, created_at
            FROM users
            WHERE user_id = $1
            """);
        cmd.Parameters.AddWithValue(userId);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new User
        {
            UserId = reader.GetGuid(0),
            PhoneNumber = reader.GetString(1),
            Username = reader.GetString(2),
            Email = reader.IsDBNull(3) ? null : reader.GetString(3),
            PasswordHash = reader.GetString(4),
            FullName = reader.IsDBNull(5) ? null : reader.GetString(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT user_id, phone_number, username, email, password_hash, full_name, created_at
            FROM users
            WHERE email = $1
            """);
        cmd.Parameters.AddWithValue(email);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new User
        {
            UserId = reader.GetGuid(0),
            PhoneNumber = reader.GetString(1),
            Username = reader.GetString(2),
            Email = reader.IsDBNull(3) ? null : reader.GetString(3),
            PasswordHash = reader.GetString(4),
            FullName = reader.IsDBNull(5) ? null : reader.GetString(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT user_id, phone_number, username, email, password_hash, full_name, created_at
            FROM users
            WHERE phone_number = $1
            """);
        cmd.Parameters.AddWithValue(phoneNumber);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
            return null;

        return new User
        {
            UserId = reader.GetGuid(0),
            PhoneNumber = reader.GetString(1),
            Username = reader.GetString(2),
            Email = reader.IsDBNull(3) ? null : reader.GetString(3),
            PasswordHash = reader.GetString(4),
            FullName = reader.IsDBNull(5) ? null : reader.GetString(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            SELECT user_id, phone_number, username, email, password_hash, full_name, created_at
            FROM users
            ORDER BY created_at DESC
            """);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        var results = new List<User>();
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new User
            {
                UserId = reader.GetGuid(0),
                PhoneNumber = reader.GetString(1),
                Username = reader.GetString(2),
                Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                PasswordHash = reader.GetString(4),
                FullName = reader.IsDBNull(5) ? null : reader.GetString(5),
                CreatedAt = reader.GetDateTime(6)
            });
        }

        return results;
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var userId = user.UserId == Guid.Empty ? Guid.NewGuid() : user.UserId;
        var createdAt = user.CreatedAt == default ? DateTime.UtcNow : user.CreatedAt;

        await using var cmd = CreateCommand(
            """
            INSERT INTO users (user_id, phone_number, username, email, password_hash, full_name, created_at)
            VALUES ($1, $2, $3, $4, $5, $6, $7)
            """);
        cmd.Parameters.AddWithValue(userId);
        cmd.Parameters.AddWithValue(user.PhoneNumber);
        cmd.Parameters.AddWithValue(user.Username);
        cmd.Parameters.AddWithValue((object?)user.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue(user.PasswordHash);
        cmd.Parameters.AddWithValue((object?)user.FullName ?? DBNull.Value);
        cmd.Parameters.AddWithValue(createdAt);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        user.UserId = userId;
        user.CreatedAt = createdAt;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand(
            """
            UPDATE users
            SET phone_number = $2,
                username = $3,
                email = $4,
                password_hash = $5,
                full_name = $6
            WHERE user_id = $1
            """);
        cmd.Parameters.AddWithValue(user.UserId);
        cmd.Parameters.AddWithValue(user.PhoneNumber);
        cmd.Parameters.AddWithValue(user.Username);
        cmd.Parameters.AddWithValue((object?)user.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue(user.PasswordHash);
        cmd.Parameters.AddWithValue((object?)user.FullName ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await using var cmd = CreateCommand("DELETE FROM users WHERE user_id = $1");
        cmd.Parameters.AddWithValue(userId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
