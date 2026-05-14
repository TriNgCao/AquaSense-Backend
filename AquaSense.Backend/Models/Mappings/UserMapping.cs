using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Entities;

namespace AquaSense.Backend.Models.Mappings;

public static class UserMapping
{
    public static UserDto ToDto(this User entity)
    {
        return new UserDto
        {
            UserId = entity.UserId,
            PhoneNumber = entity.PhoneNumber,
            Username = entity.Username,
            Email = entity.Email,
            FullName = entity.FullName,
            CreatedAt = entity.CreatedAt
        };
    }

    public static User ToEntity(this UserDto dto)
    {
        return new User
        {
            UserId = dto.UserId,
            PhoneNumber = dto.PhoneNumber,
            Username = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            CreatedAt = dto.CreatedAt
        };
    }
}
