using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class InstructorSpecializationRepository(ApplicationDbContext context) : IInstructorSpecializationRepository
{
    public async Task<InstructorSpecialization?> GetByIdAsync(
        int instructorId, 
        InstructorType type, 
        CancellationToken cancellationToken = default)
    {
        return await context.InstructorSpecializations
            .FirstOrDefaultAsync(
                x => x.InstructorId == instructorId && x.Type == type,
                cancellationToken);
    }

    public async Task<List<InstructorSpecialization>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.InstructorSpecializations.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        InstructorSpecialization instructorSpecialization, 
        CancellationToken cancellationToken = default)
    {
        await context.InstructorSpecializations.AddAsync(instructorSpecialization, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        InstructorSpecialization instructorSpecialization, 
        CancellationToken cancellationToken = default)
    {
        context.InstructorSpecializations.Update(instructorSpecialization);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        InstructorSpecialization instructorSpecialization, 
        CancellationToken cancellationToken = default)
    {
        context.InstructorSpecializations.Remove(instructorSpecialization);
        await context.SaveChangesAsync(cancellationToken);
    }
}