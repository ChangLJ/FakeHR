using HumanResource.Api.Entities;

namespace HumanResource.Api.Data;

public static class DemoApplicationFactory
{
    public static bool IsDemoFakeData(JobApplication app) =>
        !string.IsNullOrEmpty(app.Email) &&
        app.Email.Contains("@demo.example.com", StringComparison.OrdinalIgnoreCase);

    public static JobApplication CreateApplication(User user)
    {
        var chineseName = DemoDataGenerator.RandomChineseName();
        var englishName = DemoDataGenerator.RandomEnglishName();
        var city = DemoDataGenerator.Pick(DemoDataGenerator.Cities);
        var district = DemoDataGenerator.Pick(DemoDataGenerator.Districts);
        var birthYear = DemoDataGenerator.RandomYear(1985, 1998);

        user.ChineseName = chineseName;
        user.EnglishName = englishName;

        var app = new JobApplication
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RecruitPosition = $"{DemoDataGenerator.Pick(DemoDataGenerator.Positions)}（{city}）",
            IdNumber = user.IdNumber,
            FormDate = DateOnly.FromDateTime(DateTime.Today),
            ChineseName = chineseName,
            EnglishName = englishName,
            MarriageStatus = DemoDataGenerator.Pick("0", "1"),
            Gender = DemoDataGenerator.Pick("1", "2"),
            BloodType = DemoDataGenerator.Pick("A", "B", "O", "AB"),
            BirthYear = birthYear,
            BirthMonth = DemoDataGenerator.RandomMonth(),
            BirthDay = DemoDataGenerator.RandomDay(),
            BirthPlace = city,
            Email = DemoDataGenerator.RandomEmail("applicant"),
            MobilePhone = DemoDataGenerator.RandomMobile(),
            HomePhoneArea = "02",
            HomePhoneNumber = DemoDataGenerator.RandomMobile()[^8..],
            DisabilityRank = "0",
            AboriginalStatus = "N",
            HomeZipCode = DemoDataGenerator.RandomZip(),
            HomeCity = city,
            HomeDistrict = district,
            HomeAddress = DemoDataGenerator.RandomStreet(),
            MailingZipCode = DemoDataGenerator.RandomZip(),
            MailingCity = city,
            MailingDistrict = district,
            MailingAddress = DemoDataGenerator.RandomStreet(),
            EmergencyRelation = DemoDataGenerator.Pick("父母", "兄弟", "姊妹"),
            EmergencyName = DemoDataGenerator.RandomChineseName(),
            EmergencyPhone1Area = "09",
            EmergencyPhone1Number = DemoDataGenerator.RandomMobile()[^8..],
            SourceChannel = "1",
            SourceMemo = DemoDataGenerator.Pick("104", "1111", "公司官網", "朋友介紹"),
            ArmyType = DemoDataGenerator.Pick("0", "3"),
            AgreedToTerms = true,
            AllowContactCurrentEmployer = false,
            Interests = DemoDataGenerator.Pick(
                "閱讀、慢跑、攝影",
                "登山、旅行、音樂",
                "程式開發、開源專案、桌遊"),
            FuturePlan = "持續精進後端與雲端架構，三年內成為團隊技術骨幹。",
            ApplyReason = "認同公司產品方向，希望以實務經驗貢獻團隊。",
            Autobiography = "此為系統自動產生的示範自傳內容，僅供測試使用。",
            SalaryByCompanyRule = false,
            SalaryNegotiable = true,
            EarliestStartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30))
        };

        app.Educations.Add(new EducationRecord
        {
            Id = Guid.NewGuid(),
            ApplicationId = app.Id,
            SortOrder = 0,
            DegreeType = "3",
            SchoolName = DemoDataGenerator.Pick(DemoDataGenerator.Schools),
            Major = DemoDataGenerator.Pick(DemoDataGenerator.Majors),
            IsDayDivision = "Y",
            IsGraduated = "Y",
            StartYear = birthYear + 18,
            StartMonth = 9,
            EndYear = birthYear + 22,
            EndMonth = 6
        });

        app.Languages.Add(new LanguageSkill
        {
            Id = Guid.NewGuid(),
            ApplicationId = app.Id,
            SortOrder = 0,
            LanguageClass = "A",
            ListeningLevel = "2",
            SpeakingLevel = "2",
            ReadingLevel = "2",
            WritingLevel = "2"
        });

        app.FamilyMembers.Add(new FamilyMember
        {
            Id = Guid.NewGuid(),
            ApplicationId = app.Id,
            SortOrder = 0,
            Relation = "02",
            Name = DemoDataGenerator.RandomChineseName(),
            Age = DemoDataGenerator.RandomYear(55, 70).ToString(),
            Company = DemoDataGenerator.Pick(DemoDataGenerator.Companies)
        });

        app.WorkExperiences.Add(new WorkExperience
        {
            Id = Guid.NewGuid(),
            ApplicationId = app.Id,
            SortOrder = 0,
            CompanyName = DemoDataGenerator.Pick(DemoDataGenerator.Companies),
            JobTitle = DemoDataGenerator.Pick(DemoDataGenerator.JobTitles),
            IndustryType = "資訊服務業",
            MonthlySalary = (DemoDataGenerator.RandomYear(45, 80) * 1000).ToString(),
            StartYear = DemoDataGenerator.RandomYear(2018, 2022),
            StartMonth = 3,
            EndYear = DateTime.Today.Year,
            EndMonth = DateTime.Today.Month,
            LeaveReason = "尋求新挑戰",
            IsVoluntaryLeave = "Y",
            SupervisorName = DemoDataGenerator.RandomChineseName(),
            SupervisorTitle = "技術主管",
            WorkDescription = "參與內部系統開發與維運，負責 API 與資料庫設計。"
        });

        for (var i = 0; i < 2; i++)
        {
            app.References.Add(new ReferenceContact
            {
                Id = Guid.NewGuid(),
                ApplicationId = app.Id,
                SortOrder = i,
                Name = DemoDataGenerator.RandomChineseName(),
                Relation = DemoDataGenerator.Pick("主管", "同事", "客戶"),
                Company = DemoDataGenerator.Pick(DemoDataGenerator.Companies),
                JobTitle = DemoDataGenerator.Pick("經理", "課長", "資深工程師"),
                MobilePhone = DemoDataGenerator.RandomMobile()
            });
        }

        return app;
    }
}
