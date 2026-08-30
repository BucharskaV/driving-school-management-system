namespace DrivingSchool.Domain.Models;

public class RefreshToken
{
    public int Id { get; private init; }
    public int UserId { get; private set; }
    public string Token { get; private init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private init; }
    public DateTime? RevokedAtUtc { get; private set; }
    public User User { get; private init; } = null!;
 
    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTime.UtcNow;
 
    protected RefreshToken() { }
 
    public RefreshToken(User user, string token, DateTime expiresAtUtc)
    {
        User = user;
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
    }
 
    public void Revoke() => RevokedAtUtc ??= DateTime.UtcNow;
}