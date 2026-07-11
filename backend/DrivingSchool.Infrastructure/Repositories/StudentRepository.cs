using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class StudentRepository(ApplicationDbContext context) : IStudentRepository
{
    public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Users.OfType<Student>()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users.OfType<Student>()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(student, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
    {
        context.Users.Update(student);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Student student, CancellationToken cancellationToken = default)
    {
        context.Users.Remove(student);
        await context.SaveChangesAsync(cancellationToken);
    }
}