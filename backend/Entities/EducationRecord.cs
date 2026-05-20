namespace HumanResource.Api.Entities;

public class EducationRecord
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public int SortOrder { get; set; }
    public string? DegreeType { get; set; }
    public string? SchoolName { get; set; }
    public string? Major { get; set; }
    public string? Minor { get; set; }
    public string? IsDayDivision { get; set; }
    public string? IsGraduated { get; set; }
    public int? StartYear { get; set; }
    public int? StartMonth { get; set; }
    public int? EndYear { get; set; }
    public int? EndMonth { get; set; }

    public JobApplication Application { get; set; } = null!;
}
