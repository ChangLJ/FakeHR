namespace HumanResource.Api.Entities;

public class JobApplication
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    // Page 1 - Basic
    public string? RecruitPosition { get; set; }
    public string IdNumber { get; set; } = string.Empty;
    public DateOnly? FormDate { get; set; }
    public string? ChineseName { get; set; }
    public string? EnglishName { get; set; }
    public string? MarriageStatus { get; set; }
    public string? Gender { get; set; }
    public string? BloodType { get; set; }
    public int? BirthYear { get; set; }
    public int? BirthMonth { get; set; }
    public int? BirthDay { get; set; }
    public string? BirthPlace { get; set; }
    public string? Email { get; set; }
    public string? MobilePhone { get; set; }
    public string? HomePhoneArea { get; set; }
    public string? HomePhoneNumber { get; set; }
    public string? HomePhoneExt { get; set; }
    public string? ContactPhoneArea { get; set; }
    public string? ContactPhoneNumber { get; set; }
    public string? ContactPhoneExt { get; set; }
    public string? DisabilityRank { get; set; }
    public string? AboriginalStatus { get; set; }
    public string? HomeZipCode { get; set; }
    public string? HomeCity { get; set; }
    public string? HomeDistrict { get; set; }
    public string? HomeAddress { get; set; }
    public string? MailingZipCode { get; set; }
    public string? MailingCity { get; set; }
    public string? MailingDistrict { get; set; }
    public string? MailingAddress { get; set; }
    public string? EmergencyRelation { get; set; }
    public string? EmergencyName { get; set; }
    public string? EmergencyPhone1Area { get; set; }
    public string? EmergencyPhone1Number { get; set; }
    public string? EmergencyPhone1Ext { get; set; }
    public string? EmergencyPhone2Area { get; set; }
    public string? EmergencyPhone2Number { get; set; }
    public string? EmergencyPhone2Ext { get; set; }
    public string? EmergencyAddress { get; set; }
    public string? SourceChannel { get; set; }
    public string? SourceMemo { get; set; }
    public string? ArmyType { get; set; }
    public string? ArmyOtherReason { get; set; }
    public DateOnly? ArmyPeriodStart { get; set; }
    public DateOnly? ArmyPeriodEnd { get; set; }
    public string? ArmyClass { get; set; }
    public string? ArmyBranch { get; set; }
    public bool AgreedToTerms { get; set; }

    // Page 2
    public bool AllowContactCurrentEmployer { get; set; }

    // Page 3
    public string? Interests { get; set; }
    public string? FuturePlan { get; set; }
    public string? ApplyReason { get; set; }
    public string? Autobiography { get; set; }
    public bool DomesticTravelWilling { get; set; }
    public string? DomesticTravelPlace { get; set; }
    public bool OverseasTravelWilling { get; set; }
    public string? OverseasTravelPlace { get; set; }
    public string? ExpectedMonthlySalary { get; set; }
    public string? ExpectedYearlySalary { get; set; }
    public bool SalaryByCompanyRule { get; set; }
    public bool SalaryNegotiable { get; set; }
    public DateOnly? EarliestStartDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<EducationRecord> Educations { get; set; } = [];
    public ICollection<LanguageSkill> Languages { get; set; } = [];
    public ICollection<FamilyMember> FamilyMembers { get; set; } = [];
    public ICollection<WorkExperience> WorkExperiences { get; set; } = [];
    public ICollection<Certificate> Certificates { get; set; } = [];
    public ICollection<ReferenceContact> References { get; set; } = [];
}
