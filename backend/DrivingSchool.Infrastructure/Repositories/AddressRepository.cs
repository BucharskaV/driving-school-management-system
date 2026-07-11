using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class AddressRepository(ApplicationDbContext context) : IAddressRepository
{
    public async Task<Address?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Addresses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<Address>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Addresses.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Address address, CancellationToken cancellationToken = default)
    {
        await context.Addresses.AddAsync(address, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Address address, CancellationToken cancellationToken = default)
    {
        context.Addresses.Update(address);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Address address, CancellationToken cancellationToken = default)
    {
        context.Addresses.Remove(address);
        await context.SaveChangesAsync(cancellationToken);
    }
}