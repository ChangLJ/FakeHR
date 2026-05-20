using HumanResource.Api.Data;
using HumanResource.Api.DTOs;
using HumanResource.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Api.Services;

public class AuthService(AppDbContext db, TokenService tokenService)
{
    public async Task<AuthResponse?> LoginAsync(LoginRequest request, string? userAgent, string? ip, CancellationToken ct)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.IdNumber == request.IdNumber, ct);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        return await CreateSessionAsync(user, userAgent, ip, ct);
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request, string? userAgent, string? ip, CancellationToken ct)
    {
        if (await db.Users.AnyAsync(u => u.IdNumber == request.IdNumber, ct))
            return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            IdNumber = request.IdNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            ChineseName = request.ChineseName,
            EnglishName = request.EnglishName
        };

        db.Users.Add(user);
        db.JobApplications.Add(new JobApplication
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            IdNumber = request.IdNumber,
            ChineseName = request.ChineseName,
            EnglishName = request.EnglishName,
            FormDate = DateOnly.FromDateTime(DateTime.Today)
        });

        await db.SaveChangesAsync(ct);
        return await CreateSessionAsync(user, userAgent, ip, ct);
    }

    public async Task<bool> LogoutAsync(Guid userId, string tokenHash, CancellationToken ct)
    {
        var session = await db.UserSessions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.TokenHash == tokenHash && s.RevokedAt == null, ct);
        if (session is null) return false;
        session.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ValidateSessionAsync(Guid userId, string tokenHash, CancellationToken ct)
    {
        return await db.UserSessions.AnyAsync(s =>
            s.UserId == userId &&
            s.TokenHash == tokenHash &&
            s.RevokedAt == null &&
            s.ExpiresAt > DateTime.UtcNow, ct);
    }

    private async Task<AuthResponse> CreateSessionAsync(User user, string? userAgent, string? ip, CancellationToken ct)
    {
        var sessionId = Guid.NewGuid();
        var (token, hash, expires) = tokenService.GenerateToken(user, sessionId);

        db.UserSessions.Add(new UserSession
        {
            Id = sessionId,
            UserId = user.Id,
            TokenHash = hash,
            UserAgent = userAgent,
            IpAddress = ip,
            ExpiresAt = expires
        });

        user.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        return new AuthResponse(token, "Bearer", expires, user.Id, user.IdNumber, user.ChineseName);
    }
}
