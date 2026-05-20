namespace HumanResource.Api.Entities;

public class Certificate
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public int SortOrder { get; set; }
    public string? Name { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Level { get; set; }
    public string? Score { get; set; }

    public JobApplication Application { get; set; } = null!;
}
