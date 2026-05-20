namespace HumanResource.Api.DTOs;

public record LoginRequest(string IdNumber, string Password);
public record RegisterRequest(string IdNumber, string Password, string ChineseName, string EnglishName);
public record AuthResponse(string AccessToken, string TokenType, DateTime ExpiresAt, Guid UserId, string IdNumber, string? ChineseName);
