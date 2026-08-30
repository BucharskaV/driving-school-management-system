using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ICredentialRepository
{
    Task<UserCredential?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task AddAsync(UserCredential credential, CancellationToken cancellationToken = default);
}
