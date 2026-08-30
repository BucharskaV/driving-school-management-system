namespace DrivingSchool.Domain.Models;

public class UserCredential
{
    public int Id { get; private init; }
    public int UserId { get; private set; }
    public string PasswordHash { get; private set; }
    public User User { get; private init; } = null!;
 
    protected UserCredential()
    {
        PasswordHash = string.Empty;
    }
 
    public UserCredential(User user, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
 
        User = user;
        PasswordHash = passwordHash;
    }
 
    public void UpdatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
 
        PasswordHash = passwordHash;
    }
}