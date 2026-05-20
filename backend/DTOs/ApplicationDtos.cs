namespace HumanResource.Api.DTOs;

public record EducationDto(
    Guid? Id, int SortOrder, string? DegreeType, string? SchoolName, string? Major, string? Minor,
    string? IsDayDivision, string? IsGraduated, int? StartYear, int? StartMonth, int? EndYear, int? EndMonth);

public record LanguageDto(
    Guid? Id, int SortOrder, string? LanguageClass, string? OtherDescription,
    string? ListeningLevel, string? SpeakingLevel, string? ReadingLevel, string? WritingLevel);

public record FamilyMemberDto(Guid? Id, int SortOrder, string? Relation, string? Name, string? Age, string? Company);

public record WorkExperienceDto(
    Guid? Id, int SortOrder, string? CompanyName, string? JobTitle, string? IndustryType,
    string? MonthlySalary, string? YearlySalary, int? StartYear, int? StartMonth, int? EndYear, int? EndMonth,
    string? LeaveReason, string? IsVoluntaryLeave, string? SupervisorName, string? SupervisorTitle, string? WorkDescription);

public record CertificateDto(Guid? Id, int SortOrder, string? Name, string? LicenseNumber, string? Level, string? Score);

public record ReferenceDto(
    Guid? Id, int SortOrder, string? Name, string? Relation, string? Company, string? JobTitle,
    string? CompanyPhoneArea, string? CompanyPhoneNumber, string? CompanyPhoneExt, string? MobilePhone);

public record BasicInfoDto(
    string? RecruitPosition, string IdNumber, string? FormDate, string? ChineseName, string? EnglishName,
    string? MarriageStatus, string? Gender, string? BloodType, int? BirthYear, int? BirthMonth, int? BirthDay,
    string? BirthPlace, string? Email, string? MobilePhone,
    string? HomePhoneArea, string? HomePhoneNumber, string? HomePhoneExt,
    string? ContactPhoneArea, string? ContactPhoneNumber, string? ContactPhoneExt,
    string? DisabilityRank, string? AboriginalStatus,
    string? HomeZipCode, string? HomeCity, string? HomeDistrict, string? HomeAddress,
    string? MailingZipCode, string? MailingCity, string? MailingDistrict, string? MailingAddress,
    string? EmergencyRelation, string? EmergencyName,
    string? EmergencyPhone1Area, string? EmergencyPhone1Number, string? EmergencyPhone1Ext,
    string? EmergencyPhone2Area, string? EmergencyPhone2Number, string? EmergencyPhone2Ext,
    string? EmergencyAddress, string? SourceChannel, string? SourceMemo,
    string? ArmyType, string? ArmyOtherReason, string? ArmyPeriodStart, string? ArmyPeriodEnd,
    string? ArmyClass, string? ArmyBranch, bool AgreedToTerms,
    List<EducationDto> Educations, List<LanguageDto> Languages, List<FamilyMemberDto> FamilyMembers);

public record WorkHistoryDto(
    List<WorkExperienceDto> WorkExperiences, List<CertificateDto> Certificates,
    List<ReferenceDto> References, bool AllowContactCurrentEmployer);

public record SelfIntroDto(
    string? Interests, string? FuturePlan, string? ApplyReason, string? Autobiography,
    bool DomesticTravelWilling, string? DomesticTravelPlace,
    bool OverseasTravelWilling, string? OverseasTravelPlace,
    string? ExpectedMonthlySalary, string? ExpectedYearlySalary,
    bool SalaryByCompanyRule, bool SalaryNegotiable, string? EarliestStartDate);

public record ApplicationSaveDto(BasicInfoDto Basic, WorkHistoryDto Work, SelfIntroDto SelfIntro);

public record ApplicationResponseDto(
    Guid Id, BasicInfoDto? Basic, WorkHistoryDto? Work, SelfIntroDto? SelfIntro, DateTime UpdatedAt);
