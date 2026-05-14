using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/readings")]
public class SensorReadingController : BaseController
{
    private readonly ISensorReadingService _service;

    public SensorReadingController(ISensorReadingService service, ILogger<SensorReadingController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<SensorReadingDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var readings = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<SensorReadingDto>>.Ok(readings));
    }

    [HttpGet("{readingId:guid}")]
    public async Task<ActionResult<ApiResponse<SensorReadingDto>>> GetById(Guid readingId, CancellationToken cancellationToken)
    {
        var reading = await _service.GetByIdAsync(readingId, cancellationToken);
        if (reading is null)
            return NotFound(ApiResponse<SensorReadingDto>.Fail("Reading not found"));

        return Ok(ApiResponse<SensorReadingDto>.Ok(reading));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SensorReadingDto>>> Create([FromBody] SensorReadingRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { readingId = created.ReadingId }, ApiResponse<SensorReadingDto>.Ok(created));
    }

    [HttpDelete("{readingId:guid}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(Guid readingId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(readingId, cancellationToken);
        return Ok(ApiResponse<string>.Ok("deleted"));
    }
}
