namespace HumanResource.Api.Entities;

public class ReferenceContact
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public int SortOrder { get; set; }
    public string? Name { get; set; }
    public string? Relation { get; set; }
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public string? CompanyPhoneArea { get; set; }
    public string? CompanyPhoneNumber { get; set; }
    public string? CompanyPhoneExt { get; set; }
    public string? MobilePhone { get; set; }

    public JobApplication Application { get; set; } = null!;
}
