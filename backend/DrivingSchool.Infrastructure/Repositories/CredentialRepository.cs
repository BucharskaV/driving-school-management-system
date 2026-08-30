using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class CredentialRepository(ApplicationDbContext dbContext) : ICredentialRepository
{
    public Task<UserCredential?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default) =>
        dbContext.UserCredentials.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
 
    public async Task AddAsync(UserCredential credential, CancellationToken cancellationToken = default) =>
        await dbContext.UserCredentials.AddAsync(credential, cancellationToken);
}