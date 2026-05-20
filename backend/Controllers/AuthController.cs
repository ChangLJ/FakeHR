using System.Security.Claims;
using HumanResource.Api.DTOs;
using HumanResource.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResource.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await authService.LoginAsync(request, Request.Headers.UserAgent, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        return result is null ? Unauthorized(new { message = "身分證號或密碼錯誤" }) : Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await authService.RegisterAsync(request, Request.Headers.UserAgent, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        return result is null ? Conflict(new { message = "此身分證號已註冊" }) : Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var auth = Request.Headers.Authorization.ToString();
        if (!auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return BadRequest();

        var token = auth["Bearer ".Length..];
        var sub = User.FindFirstValue("sub");
        if (sub is null) return Unauthorized();
        var userId = Guid.Parse(sub);

        await authService.LogoutAsync(userId, TokenService.HashToken(token), ct);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        UserId = User.FindFirstValue("sub"),
        IdNumber = User.FindFirstValue("id_number"),
        Name = User.FindFirstValue(ClaimTypes.Name)
    });
}
