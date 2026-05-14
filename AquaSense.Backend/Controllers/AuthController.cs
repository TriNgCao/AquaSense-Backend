using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service, ILogger<AuthController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.RegisterAsync(request, cancellationToken);
            return Ok(ApiResponse<AuthResponse>.Ok(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AuthResponse>.Fail(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.LoginAsync(request, cancellationToken);
        if (result is null)
            return Unauthorized(ApiResponse<AuthResponse>.Fail("Invalid credentials"));

        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }
}
