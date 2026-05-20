namespace HumanResource.Api.Entities;

public class WorkExperience
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public int SortOrder { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string? IndustryType { get; set; }
    public string? MonthlySalary { get; set; }
    public string? YearlySalary { get; set; }
    public int? StartYear { get; set; }
    public int? StartMonth { get; set; }
    public int? EndYear { get; set; }
    public int? EndMonth { get; set; }
    public string? LeaveReason { get; set; }
    public string? IsVoluntaryLeave { get; set; }
    public string? SupervisorName { get; set; }
    public string? SupervisorTitle { get; set; }
    public string? WorkDescription { get; set; }

    public JobApplication Application { get; set; } = null!;
}
