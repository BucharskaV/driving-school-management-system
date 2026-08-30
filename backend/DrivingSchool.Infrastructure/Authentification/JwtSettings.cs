namespace DrivingSchool.Infrastructure.Authentification;

public class JwtSettings
{
    public const string SectionName = "Jwt";
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }
    public int AccessTokenExpirationMinutes { get; init; } = 15;
}