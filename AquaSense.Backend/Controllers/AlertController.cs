using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/alerts")]
public class AlertController : BaseController
{
    private readonly IAlertService _service;

    public AlertController(IAlertService service, ILogger<AlertController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AlertDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var alerts = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<AlertDto>>.Ok(alerts));
    }

    [HttpGet("{alertId:guid}")]
    public async Task<ActionResult<ApiResponse<AlertDto>>> GetById(Guid alertId, CancellationToken cancellationToken)
    {
        var alert = await _service.GetByIdAsync(alertId, cancellationToken);
        if (alert is null)
            return NotFound(ApiResponse<AlertDto>.Fail("Alert not found"));

        return Ok(ApiResponse<AlertDto>.Ok(alert));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AlertDto>>> Create([FromBody] AlertRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { alertId = created.AlertId }, ApiResponse<AlertDto>.Ok(created));
    }

    [HttpPut("{alertId:guid}")]
    public async Task<ActionResult<ApiResponse<AlertDto>>> Update(Guid alertId, [FromBody] AlertRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(alertId, request, cancellationToken);
        return Ok(ApiResponse<AlertDto>.Ok(updated));
    }

    [HttpDelete("{alertId:guid}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(Guid alertId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(alertId, cancellationToken);
        return Ok(ApiResponse<string>.Ok("deleted"));
    }
}
