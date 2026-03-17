namespace Bankampanya.Api.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "Bankampanya.Api";
    public string Audience { get; set; } = "Bankampanya.Mobile";
    public string SigningKey { get; set; } = "bankampanya-dev-signing-key-change-me-1234567890";
    public int AccessTokenLifetimeMinutes { get; set; } = 480;
}
