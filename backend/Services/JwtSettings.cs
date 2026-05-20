namespace HumanResource.Api.Services;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "HumanResource.Api";
    public string Audience { get; set; } = "HumanResource.Web";
    public int ExpirationMinutes { get; set; } = 480;
}
