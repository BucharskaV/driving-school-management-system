using DrivingSchool.Domain.Common;
using DrivingSchool.Services.Contracts.Requests.Auth;
using DrivingSchool.Services.Contracts.Responses;

namespace DrivingSchool.Services.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(int userId, CancellationToken cancellationToken = default);
}
