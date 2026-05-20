namespace HumanResource.Api.Entities;

public class User
{
    public Guid Id { get; set; }
    public string IdNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ChineseName { get; set; }
    public string? EnglishName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public JobApplication? Application { get; set; }
    public ICollection<UserSession> Sessions { get; set; } = [];
}
