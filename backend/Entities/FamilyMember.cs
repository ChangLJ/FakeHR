namespace HumanResource.Api.Entities;

public class FamilyMember
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public int SortOrder { get; set; }
    public string? Relation { get; set; }
    public string? Name { get; set; }
    public string? Age { get; set; }
    public string? Company { get; set; }

    public JobApplication Application { get; set; } = null!;
}
