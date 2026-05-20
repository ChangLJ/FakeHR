using HumanResource.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<EducationRecord> EducationRecords => Set<EducationRecord>();
    public DbSet<LanguageSkill> LanguageSkills => Set<LanguageSkill>();
    public DbSet<FamilyMember> FamilyMembers => Set<FamilyMember>();
    public DbSet<WorkExperience> WorkExperiences => Set<WorkExperience>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<ReferenceContact> ReferenceContacts => Set<ReferenceContact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.IdNumber).IsUnique();
            e.Property(x => x.IdNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<UserSession>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.TokenHash);
            e.HasOne(x => x.User).WithMany(u => u.Sessions).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<JobApplication>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.User).WithOne(u => u.Application).HasForeignKey<JobApplication>(x => x.UserId);
        });

        modelBuilder.Entity<EducationRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Application).WithMany(a => a.Educations).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LanguageSkill>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Application).WithMany(a => a.Languages).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FamilyMember>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Application).WithMany(a => a.FamilyMembers).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkExperience>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Application).WithMany(a => a.WorkExperiences).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Certificate>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Application).WithMany(a => a.Certificates).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReferenceContact>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Application).WithMany(a => a.References).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
