using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResource.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    ChineseName = table.Column<string>(type: "TEXT", nullable: true),
                    EnglishName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecruitPosition = table.Column<string>(type: "TEXT", nullable: true),
                    IdNumber = table.Column<string>(type: "TEXT", nullable: false),
                    FormDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ChineseName = table.Column<string>(type: "TEXT", nullable: true),
                    EnglishName = table.Column<string>(type: "TEXT", nullable: true),
                    MarriageStatus = table.Column<string>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", nullable: true),
                    BloodType = table.Column<string>(type: "TEXT", nullable: true),
                    BirthYear = table.Column<int>(type: "INTEGER", nullable: true),
                    BirthMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    BirthDay = table.Column<int>(type: "INTEGER", nullable: true),
                    BirthPlace = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    MobilePhone = table.Column<string>(type: "TEXT", nullable: true),
                    HomePhoneArea = table.Column<string>(type: "TEXT", nullable: true),
                    HomePhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    HomePhoneExt = table.Column<string>(type: "TEXT", nullable: true),
                    ContactPhoneArea = table.Column<string>(type: "TEXT", nullable: true),
                    ContactPhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    ContactPhoneExt = table.Column<string>(type: "TEXT", nullable: true),
                    DisabilityRank = table.Column<string>(type: "TEXT", nullable: true),
                    AboriginalStatus = table.Column<string>(type: "TEXT", nullable: true),
                    HomeZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    HomeCity = table.Column<string>(type: "TEXT", nullable: true),
                    HomeDistrict = table.Column<string>(type: "TEXT", nullable: true),
                    HomeAddress = table.Column<string>(type: "TEXT", nullable: true),
                    MailingZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    MailingCity = table.Column<string>(type: "TEXT", nullable: true),
                    MailingDistrict = table.Column<string>(type: "TEXT", nullable: true),
                    MailingAddress = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyRelation = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyName = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyPhone1Area = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyPhone1Number = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyPhone1Ext = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyPhone2Area = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyPhone2Number = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyPhone2Ext = table.Column<string>(type: "TEXT", nullable: true),
                    EmergencyAddress = table.Column<string>(type: "TEXT", nullable: true),
                    SourceChannel = table.Column<string>(type: "TEXT", nullable: true),
                    SourceMemo = table.Column<string>(type: "TEXT", nullable: true),
                    ArmyType = table.Column<string>(type: "TEXT", nullable: true),
                    ArmyOtherReason = table.Column<string>(type: "TEXT", nullable: true),
                    ArmyPeriodStart = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ArmyPeriodEnd = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ArmyClass = table.Column<string>(type: "TEXT", nullable: true),
                    ArmyBranch = table.Column<string>(type: "TEXT", nullable: true),
                    AgreedToTerms = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowContactCurrentEmployer = table.Column<bool>(type: "INTEGER", nullable: false),
                    Interests = table.Column<string>(type: "TEXT", nullable: true),
                    FuturePlan = table.Column<string>(type: "TEXT", nullable: true),
                    ApplyReason = table.Column<string>(type: "TEXT", nullable: true),
                    Autobiography = table.Column<string>(type: "TEXT", nullable: true),
                    DomesticTravelWilling = table.Column<bool>(type: "INTEGER", nullable: false),
                    DomesticTravelPlace = table.Column<string>(type: "TEXT", nullable: true),
                    OverseasTravelWilling = table.Column<bool>(type: "INTEGER", nullable: false),
                    OverseasTravelPlace = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedMonthlySalary = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedYearlySalary = table.Column<string>(type: "TEXT", nullable: true),
                    SalaryByCompanyRule = table.Column<bool>(type: "INTEGER", nullable: false),
                    SalaryNegotiable = table.Column<bool>(type: "INTEGER", nullable: false),
                    EarliestStartDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TokenHash = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    LicenseNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    Score = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DegreeType = table.Column<string>(type: "TEXT", nullable: true),
                    SchoolName = table.Column<string>(type: "TEXT", nullable: true),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    IsDayDivision = table.Column<string>(type: "TEXT", nullable: true),
                    IsGraduated = table.Column<string>(type: "TEXT", nullable: true),
                    StartYear = table.Column<int>(type: "INTEGER", nullable: true),
                    StartMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    EndYear = table.Column<int>(type: "INTEGER", nullable: true),
                    EndMonth = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationRecords_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Relation = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<string>(type: "TEXT", nullable: true),
                    Company = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageSkills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    LanguageClass = table.Column<string>(type: "TEXT", nullable: true),
                    OtherDescription = table.Column<string>(type: "TEXT", nullable: true),
                    ListeningLevel = table.Column<string>(type: "TEXT", nullable: true),
                    SpeakingLevel = table.Column<string>(type: "TEXT", nullable: true),
                    ReadingLevel = table.Column<string>(type: "TEXT", nullable: true),
                    WritingLevel = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageSkills_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Relation = table.Column<string>(type: "TEXT", nullable: true),
                    Company = table.Column<string>(type: "TEXT", nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyPhoneArea = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyPhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyPhoneExt = table.Column<string>(type: "TEXT", nullable: true),
                    MobilePhone = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferenceContacts_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: true),
                    IndustryType = table.Column<string>(type: "TEXT", nullable: true),
                    MonthlySalary = table.Column<string>(type: "TEXT", nullable: true),
                    YearlySalary = table.Column<string>(type: "TEXT", nullable: true),
                    StartYear = table.Column<int>(type: "INTEGER", nullable: true),
                    StartMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    EndYear = table.Column<int>(type: "INTEGER", nullable: true),
                    EndMonth = table.Column<int>(type: "INTEGER", nullable: true),
                    LeaveReason = table.Column<string>(type: "TEXT", nullable: true),
                    IsVoluntaryLeave = table.Column<string>(type: "TEXT", nullable: true),
                    SupervisorName = table.Column<string>(type: "TEXT", nullable: true),
                    SupervisorTitle = table.Column<string>(type: "TEXT", nullable: true),
                    WorkDescription = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_JobApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ApplicationId",
                table: "Certificates",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationRecords_ApplicationId",
                table: "EducationRecords",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_ApplicationId",
                table: "FamilyMembers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageSkills_ApplicationId",
                table: "LanguageSkills",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceContacts_ApplicationId",
                table: "ReferenceContacts",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdNumber",
                table: "Users",
                column: "IdNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_TokenHash",
                table: "UserSessions",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_ApplicationId",
                table: "WorkExperiences",
                column: "ApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "EducationRecords");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "LanguageSkills");

            migrationBuilder.DropTable(
                name: "ReferenceContacts");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
