using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;

namespace AquaSense.Backend.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(UserRequest request, CancellationToken cancellationToken = default);
    Task<UserDto> UpdateAsync(Guid userId, UserRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
}
