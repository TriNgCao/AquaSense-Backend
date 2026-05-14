using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;
using AquaSense.Backend.Models.Mappings;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Repositories.Interfaces;
using AquaSense.Backend.Services.Interfaces;

namespace AquaSense.Backend.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdAsync(userId, cancellationToken);
        return user?.ToDto();
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _repository.GetAllAsync(cancellationToken);
        return users.Select(u => u.ToDto()).ToList();
    }

    public async Task<UserDto> CreateAsync(UserRequest request, CancellationToken cancellationToken = default)
    {
        var username = string.IsNullOrWhiteSpace(request.Username)
            ? request.PhoneNumber
            : request.Username;

        var entity = new User
        {
            PhoneNumber = request.PhoneNumber,
            Username = username,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email,
            PasswordHash = request.Password,
            FullName = request.FullName
        };

        await _repository.CreateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public async Task<UserDto> UpdateAsync(Guid userId, UserRequest request, CancellationToken cancellationToken = default)
    {
        var username = string.IsNullOrWhiteSpace(request.Username)
            ? request.PhoneNumber
            : request.Username;

        var entity = new User
        {
            UserId = userId,
            PhoneNumber = request.PhoneNumber,
            Username = username,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email,
            PasswordHash = request.Password,
            FullName = request.FullName
        };

        await _repository.UpdateAsync(entity, cancellationToken);
        return entity.ToDto();
    }

    public Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(userId, cancellationToken);
    }
}
