using DrivingSchool.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DrivingSchool.Infrastructure.Authentification;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _identityHasher = new();
 
    public string Hash(string password) => _identityHasher.HashPassword(default!, password);
 
    public bool Verify(string password, string passwordHash)
    {
        var result = _identityHasher.VerifyHashedPassword(default!, passwordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}