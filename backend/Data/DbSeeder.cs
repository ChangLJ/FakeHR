using HumanResource.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HumanResource.Api.Data;

public static class DbSeeder
{
    private const string LegacyDemoIdNumber = "E123458858";

    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        var demoUser = await db.Users.FirstOrDefaultAsync(u => u.IdNumber == DemoDataGenerator.DemoIdNumber);
        if (demoUser is not null)
        {
            EnsureDemoPassword(demoUser);
            await EnsureDemoApplicationAsync(db, demoUser, forceReset: false);
            await db.SaveChangesAsync();
            return;
        }

        var legacy = await db.Users.FirstOrDefaultAsync(u => u.IdNumber == LegacyDemoIdNumber);
        if (legacy is not null)
        {
            legacy.IdNumber = DemoDataGenerator.DemoIdNumber;
            EnsureDemoPassword(legacy);
            await EnsureDemoApplicationAsync(db, legacy, forceReset: true);
            await db.SaveChangesAsync();
            return;
        }

        if (await db.Users.AnyAsync())
            return;

        var user = new User
        {
            Id = Guid.NewGuid(),
            IdNumber = DemoDataGenerator.DemoIdNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(DemoDataGenerator.DemoPassword),
        };
        var app = DemoApplicationFactory.CreateApplication(user);
        db.Users.Add(user);
        db.JobApplications.Add(app);
        await db.SaveChangesAsync();
    }

    private static async Task EnsureDemoApplicationAsync(AppDbContext db, User user, bool forceReset)
    {
        var app = await db.JobApplications.FirstOrDefaultAsync(a => a.UserId == user.Id);
        if (app is not null && !forceReset && DemoApplicationFactory.IsDemoFakeData(app))
            return;

        if (app is not null)
        {
            db.JobApplications.Remove(app);
            await db.SaveChangesAsync();
        }

        db.JobApplications.Add(DemoApplicationFactory.CreateApplication(user));
    }

    private static void EnsureDemoPassword(User user)
    {
        if (!BCrypt.Net.BCrypt.Verify(DemoDataGenerator.DemoPassword, user.PasswordHash))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(DemoDataGenerator.DemoPassword);
    }
}
