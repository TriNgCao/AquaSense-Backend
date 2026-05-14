using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/ponds")]
public class PondController : BaseController
{
    private readonly IPondService _service;

    public PondController(IPondService service, ILogger<PondController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<PondDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var ponds = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<PondDto>>.Ok(ponds));
    }

    [HttpGet("{pondId:guid}")]
    public async Task<ActionResult<ApiResponse<PondDto>>> GetById(Guid pondId, CancellationToken cancellationToken)
    {
        var pond = await _service.GetByIdAsync(pondId, cancellationToken);
        if (pond is null)
            return NotFound(ApiResponse<PondDto>.Fail("Pond not found"));

        return Ok(ApiResponse<PondDto>.Ok(pond));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PondDto>>> Create([FromBody] PondRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { pondId = created.PondId }, ApiResponse<PondDto>.Ok(created));
    }

    [HttpPut("{pondId:guid}")]
    public async Task<ActionResult<ApiResponse<PondDto>>> Update(Guid pondId, [FromBody] PondRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(pondId, request, cancellationToken);
        return Ok(ApiResponse<PondDto>.Ok(updated));
    }

    [HttpDelete("{pondId:guid}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(Guid pondId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(pondId, cancellationToken);
        return Ok(ApiResponse<string>.Ok("deleted"));
    }
}
