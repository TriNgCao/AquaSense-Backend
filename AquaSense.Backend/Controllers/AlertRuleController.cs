using AquaSense.Backend.Models.DTOs;
using AquaSense.Backend.Models.Request;
using AquaSense.Backend.Models.Response;
using AquaSense.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[Route("api/alert-rules")]
public class AlertRuleController : BaseController
{
    private readonly IAlertRuleService _service;

    public AlertRuleController(IAlertRuleService service, ILogger<AlertRuleController> logger) : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AlertRuleDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var rules = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<AlertRuleDto>>.Ok(rules));
    }

    [HttpGet("{ruleId:guid}")]
    public async Task<ActionResult<ApiResponse<AlertRuleDto>>> GetById(Guid ruleId, CancellationToken cancellationToken)
    {
        var rule = await _service.GetByIdAsync(ruleId, cancellationToken);
        if (rule is null)
            return NotFound(ApiResponse<AlertRuleDto>.Fail("Rule not found"));

        return Ok(ApiResponse<AlertRuleDto>.Ok(rule));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AlertRuleDto>>> Create([FromBody] AlertRuleRequest request, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { ruleId = created.RuleId }, ApiResponse<AlertRuleDto>.Ok(created));
    }

    [HttpPut("{ruleId:guid}")]
    public async Task<ActionResult<ApiResponse<AlertRuleDto>>> Update(Guid ruleId, [FromBody] AlertRuleRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(ruleId, request, cancellationToken);
        return Ok(ApiResponse<AlertRuleDto>.Ok(updated));
    }

    [HttpDelete("{ruleId:guid}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(Guid ruleId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(ruleId, cancellationToken);
        return Ok(ApiResponse<string>.Ok("deleted"));
    }
}
