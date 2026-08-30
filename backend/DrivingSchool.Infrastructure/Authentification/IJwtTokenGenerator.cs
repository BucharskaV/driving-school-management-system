using DrivingSchool.Domain.Models;

namespace DrivingSchool.Infrastructure.Authentification;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAtUtc) GenerateAccessToken(User user);
    string GenerateRefreshToken();
}