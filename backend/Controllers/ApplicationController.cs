using System.Security.Claims;
using HumanResource.Api.DTOs;
using HumanResource.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResource.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/application")]
public class ApplicationController(ApplicationService applicationService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue("sub")!);

    [HttpGet]
    public async Task<ActionResult<ApplicationResponseDto>> Get(CancellationToken ct)
    {
        var result = await applicationService.GetAsync(UserId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<ApplicationResponseDto>> Save([FromBody] ApplicationSaveDto dto, CancellationToken ct)
    {
        var result = await applicationService.SaveAsync(UserId, dto, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
