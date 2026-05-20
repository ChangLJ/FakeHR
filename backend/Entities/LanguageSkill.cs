namespace HumanResource.Api.Entities;

public class LanguageSkill
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public int SortOrder { get; set; }
    public string? LanguageClass { get; set; }
    public string? OtherDescription { get; set; }
    public string? ListeningLevel { get; set; }
    public string? SpeakingLevel { get; set; }
    public string? ReadingLevel { get; set; }
    public string? WritingLevel { get; set; }

    public JobApplication Application { get; set; } = null!;
}
