using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class CertificationRepository(ApplicationDbContext context) : ICertificationRepository
{
    public async Task<Certification?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Certifications.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<Certification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Certifications.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Certification certification, CancellationToken cancellationToken = default)
    {
        await context.Certifications.AddAsync(certification, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Certification certification, CancellationToken cancellationToken = default)
    {
        context.Certifications.Update(certification);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Certification certification, CancellationToken cancellationToken = default)
    {
        context.Certifications.Remove(certification);
        await context.SaveChangesAsync(cancellationToken);
    }
}