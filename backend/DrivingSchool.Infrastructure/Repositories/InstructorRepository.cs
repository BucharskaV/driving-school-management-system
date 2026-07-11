using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class InstructorRepository(ApplicationDbContext context) : IInstructorRepository
{
    public async Task<Instructor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .OfType<Instructor>()
            .Include(i => i.Specializations)
            .Include(i => i.Certifications)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Instructor?> GetByCodeAsync(string instructorCode, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .OfType<Instructor>()
            .Include(i => i.Specializations)
            .Include(i => i.Certifications)
            .FirstOrDefaultAsync(p => p.InstructorCode == instructorCode, cancellationToken);
    }

    public async Task<List<Instructor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .OfType<Instructor>()
            .Include(i => i.Specializations)
            .Include(i => i.Certifications)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(instructor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Instructor instructor, CancellationToken cancellationToken = default)
    {
        context.Users.Update(instructor);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Instructor instructor, CancellationToken cancellationToken = default)
    {
        context.Users.Remove(instructor);
        await context.SaveChangesAsync(cancellationToken);
    }
}