using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HumanResource.Api.Data;
using HumanResource.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 僅本機開發強制 5001；Docker / Cloud Run 使用 PORT 或 ASPNETCORE_HTTP_PORTS（8080）
if (builder.Environment.IsDevelopment())
    builder.WebHost.UseUrls("http://0.0.0.0:5001");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

var dbPath = builder.Configuration["Database:Path"]
    ?? Environment.GetEnvironmentVariable("DATABASE_PATH")
    ?? Path.Combine("data", "humanresource.db");
var dbDirectory = Path.GetDirectoryName(Path.GetFullPath(dbPath));
if (!string.IsNullOrEmpty(dbDirectory))
    Directory.CreateDirectory(dbDirectory);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApplicationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var principal = context.Principal;
                if (principal?.Identity?.IsAuthenticated != true)
                {
                    context.Fail("Invalid principal");
                    return;
                }

                var userIdValue = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdValue) || !Guid.TryParse(userIdValue, out var userId))
                {
                    context.Fail("Missing user id claim");
                    return;
                }

                var authHeader = context.Request.Headers.Authorization.ToString();
                if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Fail("Missing bearer token");
                    return;
                }

                var token = authHeader["Bearer ".Length..].Trim();
                if (string.IsNullOrEmpty(token))
                {
                    context.Fail("Empty bearer token");
                    return;
                }

                var authService = context.HttpContext.RequestServices.GetRequiredService<AuthService>();
                var valid = await authService.ValidateSessionAsync(
                    userId, TokenService.HashToken(token), context.HttpContext.RequestAborted);
                if (!valid)
                    context.Fail("Session revoked or expired");
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:5174"])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
