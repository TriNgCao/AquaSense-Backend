using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/users")]
public class UserController : BaseController
{
    private readonly IUserService _service;

    public UserController(IUserService service, ILogger<UserController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<UserDto>>.Ok(users));
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _service.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return NotFound(ApiResponse<UserDto>.Fail("User not found"));

        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create([FromBody] UserRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { userId = created.UserId }, ApiResponse<UserDto>.Ok(created));
    }

    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Update(Guid userId, [FromBody] UserRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(userId, request, cancellationToken);
        return Ok(ApiResponse<UserDto>.Ok(updated));
    }

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(Guid userId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(userId, cancellationToken);
        return Ok(ApiResponse<string>.Ok("deleted"));
    }
}
