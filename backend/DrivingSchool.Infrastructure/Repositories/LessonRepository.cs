using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class LessonRepository(ApplicationDbContext context) : ILessonRepository
{
    public async Task<Lesson?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<List<Lesson>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Lesson lesson, CancellationToken cancellationToken = default)
    {
        await context.Lessons.AddAsync(lesson, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Lesson lesson, CancellationToken cancellationToken = default)
    {
        context.Lessons.Update(lesson);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Lesson lesson, CancellationToken cancellationToken = default)
    {
        context.Lessons.Remove(lesson);
        await context.SaveChangesAsync(cancellationToken);
    }
}