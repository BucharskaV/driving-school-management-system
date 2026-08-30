using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Authentification;
using DrivingSchool.Services.Contracts.Requests.Auth;
using DrivingSchool.Services.Contracts.Responses;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Authentification;

public class AuthService(
    IUserRepository userRepository,
    ICredentialRepository credentialRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator tokenGenerator) : IAuthService
{
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);
 
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure<AuthResponse>("A user with this email already exists.");
 
        var user = new User(request.FirstName, request.LastName, Role.Student, request.Pesel, request.PhoneNumber, request.Email);
        await userRepository.AddAsync(user, cancellationToken);
 
        var credential = new UserCredential(user, passwordHasher.Hash(request.Password));
        await credentialRepository.AddAsync(credential, cancellationToken);
 
        return Result.Success(await IssueTokensAsync(user, cancellationToken));
    }
 
    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return Result.Failure<AuthResponse>("Invalid email or password.");
 
        var credential = await credentialRepository.GetByUserIdAsync(user.Id, cancellationToken);
        if (credential is null || !passwordHasher.Verify(request.Password, credential.PasswordHash))
            return Result.Failure<AuthResponse>("Invalid email or password.");
 
        return Result.Success(await IssueTokensAsync(user, cancellationToken));
    }
 
    public async Task<Result<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null)
            return Result.Failure<AuthResponse>("Invalid or expired refresh token.");
 
        await userRepository.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
        return Result.Success(await IssueTokensAsync(user, cancellationToken));
    }
 
    public async Task<Result> RevokeRefreshTokenAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure("User not found.");
 
        await userRepository.RevokeAllRefreshTokensAsync(userId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
 
    private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken cancellationToken)
    {
        var (accessToken, expiresAtUtc) = tokenGenerator.GenerateAccessToken(user);
        var refreshTokenValue = tokenGenerator.GenerateRefreshToken();
 
        var refreshToken = new RefreshToken(user, refreshTokenValue, DateTime.UtcNow.Add(RefreshTokenLifetime));
        await userRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
 
        await unitOfWork.SaveChangesAsync(cancellationToken);
 
        return new AuthResponse(user.Id, user.FullName, user.Email ?? string.Empty, user.Role.ToString(), accessToken, refreshTokenValue, expiresAtUtc);
    }
}