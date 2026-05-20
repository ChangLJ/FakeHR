using HumanResource.Api.DTOs;
using HumanResource.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResource.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/review/applications")]
public class ReviewController(ApplicationService applicationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ApplicationSummaryDto>>> List(CancellationToken ct) =>
        Ok(await applicationService.ListSummariesAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApplicationResponseDto>> Get(Guid id, CancellationToken ct)
    {
        var result = await applicationService.GetByApplicationIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
