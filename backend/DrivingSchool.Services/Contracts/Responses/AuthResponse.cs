namespace DrivingSchool.Services.Contracts.Responses;

public record AuthResponse(int UserId, string FullName, string Email, string Role, string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);
