using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class EnrollmentRepository(ApplicationDbContext context) : IEnrollmentRepository
{
    public async Task<Enrollment?> GetByIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default)
    {
        return await context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken);
    }

    public async Task<List<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Enrollments
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        await context.Enrollments.AddAsync(enrollment, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        context.Enrollments.Update(enrollment);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        context.Enrollments.Remove(enrollment);
        await context.SaveChangesAsync(cancellationToken);
    }
}