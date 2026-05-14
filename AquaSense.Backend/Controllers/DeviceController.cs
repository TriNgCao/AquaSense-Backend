using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/devices")]
public class DeviceController : BaseController
{
    private readonly IDeviceService _service;

    public DeviceController(IDeviceService service, ILogger<DeviceController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DeviceDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var devices = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<DeviceDto>>.Ok(devices));
    }

    [HttpGet("{deviceId}")]
    public async Task<ActionResult<ApiResponse<DeviceDto>>> GetById(string deviceId, CancellationToken cancellationToken)
    {
        var device = await _service.GetByIdAsync(deviceId, cancellationToken);
        if (device is null)
            return NotFound(ApiResponse<DeviceDto>.Fail("Device not found"));

        return Ok(ApiResponse<DeviceDto>.Ok(device));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DeviceDto>>> Create([FromBody] DeviceRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { deviceId = created.DeviceId }, ApiResponse<DeviceDto>.Ok(created));
    }

    [HttpPut("{deviceId}")]
    public async Task<ActionResult<ApiResponse<DeviceDto>>> Update(string deviceId, [FromBody] DeviceRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(deviceId, request, cancellationToken);
        return Ok(ApiResponse<DeviceDto>.Ok(updated));
    }

    [HttpDelete("{deviceId}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(string deviceId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(deviceId, cancellationToken);
        return Ok(ApiResponse<string>.Ok("deleted"));
    }
}
