using HumanResource.Api.Data;
using HumanResource.Api.DTOs;
using HumanResource.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Api.Services;

public class ApplicationService(AppDbContext db)
{
    public async Task<ApplicationResponseDto?> GetAsync(Guid userId, CancellationToken ct)
    {
        var app = await LoadApplicationAsync(userId, ct);
        return app is null ? null : MapToResponse(app);
    }

    public async Task<List<ApplicationSummaryDto>> ListSummariesAsync(CancellationToken ct) =>
        await db.JobApplications
            .OrderByDescending(a => a.UpdatedAt)
            .Select(a => new ApplicationSummaryDto(
                a.Id,
                a.IdNumber,
                a.ChineseName,
                a.EnglishName,
                a.RecruitPosition,
                a.Email,
                a.UpdatedAt))
            .ToListAsync(ct);

    public async Task<ApplicationResponseDto?> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct)
    {
        var app = await LoadApplicationByIdAsync(applicationId, ct);
        return app is null ? null : MapToResponse(app);
    }

    public async Task<ApplicationResponseDto?> SaveAsync(Guid userId, ApplicationSaveDto dto, CancellationToken ct)
    {
        var app = await GetOrCreateApplicationForUpdateAsync(userId, ct);
        var appId = app.Id;

        await DeleteAllChildrenAsync(appId, ct);
        db.ChangeTracker.Clear();

        app = await db.JobApplications.FirstAsync(a => a.Id == appId, ct);
        ApplyBasic(app, dto.Basic);
        InsertEducations(appId, dto.Basic.Educations);
        InsertLanguages(appId, dto.Basic.Languages);
        InsertFamilyMembers(appId, dto.Basic.FamilyMembers);

        app.AllowContactCurrentEmployer = dto.Work.AllowContactCurrentEmployer;
        InsertWorkExperiences(appId, dto.Work.WorkExperiences);
        InsertCertificates(appId, dto.Work.Certificates);
        InsertReferences(appId, dto.Work.References);

        ApplySelfIntro(app, dto.SelfIntro);
        app.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return await GetAsync(userId, ct);
    }

    private async Task<JobApplication?> LoadApplicationAsync(Guid userId, CancellationToken ct) =>
        await QueryApplicationWithChildren()
            .FirstOrDefaultAsync(a => a.UserId == userId, ct);

    private async Task<JobApplication?> LoadApplicationByIdAsync(Guid applicationId, CancellationToken ct) =>
        await QueryApplicationWithChildren()
            .FirstOrDefaultAsync(a => a.Id == applicationId, ct);

    private IQueryable<JobApplication> QueryApplicationWithChildren() =>
        db.JobApplications
            .Include(a => a.Educations.OrderBy(e => e.SortOrder))
            .Include(a => a.Languages.OrderBy(l => l.SortOrder))
            .Include(a => a.FamilyMembers.OrderBy(f => f.SortOrder))
            .Include(a => a.WorkExperiences.OrderBy(w => w.SortOrder))
            .Include(a => a.Certificates.OrderBy(c => c.SortOrder))
            .Include(a => a.References.OrderBy(r => r.SortOrder));

    private async Task<JobApplication> GetOrCreateApplicationForUpdateAsync(Guid userId, CancellationToken ct)
    {
        var existing = await db.JobApplications.FirstOrDefaultAsync(a => a.UserId == userId, ct);
        if (existing is not null) return existing;

        var user = await db.Users.FindAsync([userId], ct)
            ?? throw new InvalidOperationException("User not found");

        var app = new JobApplication
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            IdNumber = user.IdNumber,
            ChineseName = user.ChineseName,
            EnglishName = user.EnglishName,
            FormDate = DateOnly.FromDateTime(DateTime.Today)
        };
        db.JobApplications.Add(app);
        await db.SaveChangesAsync(ct);
        return app;
    }

    private async Task DeleteAllChildrenAsync(Guid appId, CancellationToken ct)
    {
        await db.EducationRecords.Where(e => e.ApplicationId == appId).ExecuteDeleteAsync(ct);
        await db.LanguageSkills.Where(l => l.ApplicationId == appId).ExecuteDeleteAsync(ct);
        await db.FamilyMembers.Where(f => f.ApplicationId == appId).ExecuteDeleteAsync(ct);
        await db.WorkExperiences.Where(w => w.ApplicationId == appId).ExecuteDeleteAsync(ct);
        await db.Certificates.Where(c => c.ApplicationId == appId).ExecuteDeleteAsync(ct);
        await db.ReferenceContacts.Where(r => r.ApplicationId == appId).ExecuteDeleteAsync(ct);
    }

    private static void ApplySelfIntro(JobApplication app, SelfIntroDto dto)
    {
        app.Interests = dto.Interests;
        app.FuturePlan = dto.FuturePlan;
        app.ApplyReason = dto.ApplyReason;
        app.Autobiography = dto.Autobiography;
        app.DomesticTravelWilling = dto.DomesticTravelWilling;
        app.DomesticTravelPlace = dto.DomesticTravelPlace;
        app.OverseasTravelWilling = dto.OverseasTravelWilling;
        app.OverseasTravelPlace = dto.OverseasTravelPlace;
        app.ExpectedMonthlySalary = dto.ExpectedMonthlySalary;
        app.ExpectedYearlySalary = dto.ExpectedYearlySalary;
        app.SalaryByCompanyRule = dto.SalaryByCompanyRule;
        app.SalaryNegotiable = dto.SalaryNegotiable;
        app.EarliestStartDate = ParseDate(dto.EarliestStartDate);
    }

    private static void ApplyBasic(JobApplication app, BasicInfoDto dto)
    {
        app.RecruitPosition = dto.RecruitPosition;
        app.IdNumber = dto.IdNumber;
        app.FormDate = ParseDate(dto.FormDate) ?? app.FormDate;
        app.ChineseName = dto.ChineseName;
        app.EnglishName = dto.EnglishName;
        app.MarriageStatus = dto.MarriageStatus;
        app.Gender = dto.Gender;
        app.BloodType = dto.BloodType;
        app.BirthYear = dto.BirthYear;
        app.BirthMonth = dto.BirthMonth;
        app.BirthDay = dto.BirthDay;
        app.BirthPlace = dto.BirthPlace;
        app.Email = dto.Email;
        app.MobilePhone = dto.MobilePhone;
        app.HomePhoneArea = dto.HomePhoneArea;
        app.HomePhoneNumber = dto.HomePhoneNumber;
        app.HomePhoneExt = dto.HomePhoneExt;
        app.ContactPhoneArea = dto.ContactPhoneArea;
        app.ContactPhoneNumber = dto.ContactPhoneNumber;
        app.ContactPhoneExt = dto.ContactPhoneExt;
        app.DisabilityRank = dto.DisabilityRank;
        app.AboriginalStatus = dto.AboriginalStatus;
        app.HomeZipCode = dto.HomeZipCode;
        app.HomeCity = dto.HomeCity;
        app.HomeDistrict = dto.HomeDistrict;
        app.HomeAddress = dto.HomeAddress;
        app.MailingZipCode = dto.MailingZipCode;
        app.MailingCity = dto.MailingCity;
        app.MailingDistrict = dto.MailingDistrict;
        app.MailingAddress = dto.MailingAddress;
        app.EmergencyRelation = dto.EmergencyRelation;
        app.EmergencyName = dto.EmergencyName;
        app.EmergencyPhone1Area = dto.EmergencyPhone1Area;
        app.EmergencyPhone1Number = dto.EmergencyPhone1Number;
        app.EmergencyPhone1Ext = dto.EmergencyPhone1Ext;
        app.EmergencyPhone2Area = dto.EmergencyPhone2Area;
        app.EmergencyPhone2Number = dto.EmergencyPhone2Number;
        app.EmergencyPhone2Ext = dto.EmergencyPhone2Ext;
        app.EmergencyAddress = dto.EmergencyAddress;
        app.SourceChannel = dto.SourceChannel;
        app.SourceMemo = dto.SourceMemo;
        app.ArmyType = dto.ArmyType;
        app.ArmyOtherReason = dto.ArmyOtherReason;
        app.ArmyPeriodStart = ParseDate(dto.ArmyPeriodStart);
        app.ArmyPeriodEnd = ParseDate(dto.ArmyPeriodEnd);
        app.ArmyClass = dto.ArmyClass;
        app.ArmyBranch = dto.ArmyBranch;
        app.AgreedToTerms = dto.AgreedToTerms;
    }

    private void InsertEducations(Guid appId, List<EducationDto> items)
    {
        foreach (var (item, i) in items.Select((e, i) => (e, i)))
        {
            db.EducationRecords.Add(new EducationRecord
            {
                Id = Guid.NewGuid(),
                ApplicationId = appId,
                SortOrder = i,
                DegreeType = item.DegreeType,
                SchoolName = item.SchoolName,
                Major = item.Major,
                Minor = item.Minor,
                IsDayDivision = item.IsDayDivision,
                IsGraduated = item.IsGraduated,
                StartYear = item.StartYear,
                StartMonth = item.StartMonth,
                EndYear = item.EndYear,
                EndMonth = item.EndMonth
            });
        }
    }

    private void InsertLanguages(Guid appId, List<LanguageDto> items)
    {
        foreach (var (item, i) in items.Select((l, i) => (l, i)))
        {
            db.LanguageSkills.Add(new LanguageSkill
            {
                Id = Guid.NewGuid(),
                ApplicationId = appId,
                SortOrder = i,
                LanguageClass = item.LanguageClass,
                OtherDescription = item.OtherDescription,
                ListeningLevel = item.ListeningLevel,
                SpeakingLevel = item.SpeakingLevel,
                ReadingLevel = item.ReadingLevel,
                WritingLevel = item.WritingLevel
            });
        }
    }

    private void InsertFamilyMembers(Guid appId, List<FamilyMemberDto> items)
    {
        foreach (var (item, i) in items.Select((f, i) => (f, i)))
        {
            db.FamilyMembers.Add(new FamilyMember
            {
                Id = Guid.NewGuid(),
                ApplicationId = appId,
                SortOrder = i,
                Relation = item.Relation,
                Name = item.Name,
                Age = item.Age,
                Company = item.Company
            });
        }
    }

    private void InsertWorkExperiences(Guid appId, List<WorkExperienceDto> items)
    {
        foreach (var (item, i) in items.Select((w, i) => (w, i)))
        {
            db.WorkExperiences.Add(new WorkExperience
            {
                Id = Guid.NewGuid(),
                ApplicationId = appId,
                SortOrder = i,
                CompanyName = item.CompanyName,
                JobTitle = item.JobTitle,
                IndustryType = item.IndustryType,
                MonthlySalary = item.MonthlySalary,
                YearlySalary = item.YearlySalary,
                StartYear = item.StartYear,
                StartMonth = item.StartMonth,
                EndYear = item.EndYear,
                EndMonth = item.EndMonth,
                LeaveReason = item.LeaveReason,
                IsVoluntaryLeave = item.IsVoluntaryLeave,
                SupervisorName = item.SupervisorName,
                SupervisorTitle = item.SupervisorTitle,
                WorkDescription = item.WorkDescription
            });
        }
    }

    private void InsertCertificates(Guid appId, List<CertificateDto> items)
    {
        foreach (var (item, i) in items.Select((c, i) => (c, i)))
        {
            db.Certificates.Add(new Certificate
            {
                Id = Guid.NewGuid(),
                ApplicationId = appId,
                SortOrder = i,
                Name = item.Name,
                LicenseNumber = item.LicenseNumber,
                Level = item.Level,
                Score = item.Score
            });
        }
    }

    private void InsertReferences(Guid appId, List<ReferenceDto> items)
    {
        foreach (var (item, i) in items.Select((r, i) => (r, i)))
        {
            db.ReferenceContacts.Add(new ReferenceContact
            {
                Id = Guid.NewGuid(),
                ApplicationId = appId,
                SortOrder = i,
                Name = item.Name,
                Relation = item.Relation,
                Company = item.Company,
                JobTitle = item.JobTitle,
                CompanyPhoneArea = item.CompanyPhoneArea,
                CompanyPhoneNumber = item.CompanyPhoneNumber,
                CompanyPhoneExt = item.CompanyPhoneExt,
                MobilePhone = item.MobilePhone
            });
        }
    }

    private static DateOnly? ParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (DateOnly.TryParse(value, out var d)) return d;
        if (value.Length == 8 && DateOnly.TryParseExact(value, "yyyyMMdd", out d)) return d;
        return null;
    }

    private static string? FormatDate(DateOnly? d) => d?.ToString("yyyy-MM-dd");

    private static ApplicationResponseDto MapToResponse(JobApplication app) => new(
        app.Id,
        new BasicInfoDto(
            app.RecruitPosition, app.IdNumber, FormatDate(app.FormDate), app.ChineseName, app.EnglishName,
            app.MarriageStatus, app.Gender, app.BloodType, app.BirthYear, app.BirthMonth, app.BirthDay,
            app.BirthPlace, app.Email, app.MobilePhone,
            app.HomePhoneArea, app.HomePhoneNumber, app.HomePhoneExt,
            app.ContactPhoneArea, app.ContactPhoneNumber, app.ContactPhoneExt,
            app.DisabilityRank, app.AboriginalStatus,
            app.HomeZipCode, app.HomeCity, app.HomeDistrict, app.HomeAddress,
            app.MailingZipCode, app.MailingCity, app.MailingDistrict, app.MailingAddress,
            app.EmergencyRelation, app.EmergencyName,
            app.EmergencyPhone1Area, app.EmergencyPhone1Number, app.EmergencyPhone1Ext,
            app.EmergencyPhone2Area, app.EmergencyPhone2Number, app.EmergencyPhone2Ext,
            app.EmergencyAddress, app.SourceChannel, app.SourceMemo,
            app.ArmyType, app.ArmyOtherReason, FormatDate(app.ArmyPeriodStart), FormatDate(app.ArmyPeriodEnd),
            app.ArmyClass, app.ArmyBranch, app.AgreedToTerms,
            app.Educations.Select(e => new EducationDto(e.Id, e.SortOrder, e.DegreeType, e.SchoolName, e.Major, e.Minor,
                e.IsDayDivision, e.IsGraduated, e.StartYear, e.StartMonth, e.EndYear, e.EndMonth)).ToList(),
            app.Languages.Select(l => new LanguageDto(l.Id, l.SortOrder, l.LanguageClass, l.OtherDescription,
                l.ListeningLevel, l.SpeakingLevel, l.ReadingLevel, l.WritingLevel)).ToList(),
            app.FamilyMembers.Select(f => new FamilyMemberDto(f.Id, f.SortOrder, f.Relation, f.Name, f.Age, f.Company)).ToList()),
        new WorkHistoryDto(
            app.WorkExperiences.Select(w => new WorkExperienceDto(w.Id, w.SortOrder, w.CompanyName, w.JobTitle, w.IndustryType,
                w.MonthlySalary, w.YearlySalary, w.StartYear, w.StartMonth, w.EndYear, w.EndMonth,
                w.LeaveReason, w.IsVoluntaryLeave, w.SupervisorName, w.SupervisorTitle, w.WorkDescription)).ToList(),
            app.Certificates.Select(c => new CertificateDto(c.Id, c.SortOrder, c.Name, c.LicenseNumber, c.Level, c.Score)).ToList(),
            app.References.Select(r => new ReferenceDto(r.Id, r.SortOrder, r.Name, r.Relation, r.Company, r.JobTitle,
                r.CompanyPhoneArea, r.CompanyPhoneNumber, r.CompanyPhoneExt, r.MobilePhone)).ToList(),
            app.AllowContactCurrentEmployer),
        new SelfIntroDto(
            app.Interests, app.FuturePlan, app.ApplyReason, app.Autobiography,
            app.DomesticTravelWilling, app.DomesticTravelPlace,
            app.OverseasTravelWilling, app.OverseasTravelPlace,
            app.ExpectedMonthlySalary, app.ExpectedYearlySalary,
            app.SalaryByCompanyRule, app.SalaryNegotiable, FormatDate(app.EarliestStartDate)),
        app.UpdatedAt);
}
