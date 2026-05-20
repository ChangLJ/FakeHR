namespace HumanResource.Api.DTOs;

public record ApplicationSummaryDto(
    Guid Id,
    string IdNumber,
    string? ChineseName,
    string? EnglishName,
    string? RecruitPosition,
    string? Email,
    DateTime UpdatedAt);
