using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class ExtraFeeRepository(ApplicationDbContext context) : IExtraFeeRepository
{
    public async Task<ExtraFee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.ExtraFees
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<List<ExtraFee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.ExtraFees
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ExtraFee extraFee, CancellationToken cancellationToken = default)
    {
        await context.ExtraFees.AddAsync(extraFee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ExtraFee extraFee, CancellationToken cancellationToken = default)
    {
        context.ExtraFees.Update(extraFee);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(ExtraFee extraFee, CancellationToken cancellationToken = default)
    {
        context.ExtraFees.Remove(extraFee);
        await context.SaveChangesAsync(cancellationToken);
    }
}