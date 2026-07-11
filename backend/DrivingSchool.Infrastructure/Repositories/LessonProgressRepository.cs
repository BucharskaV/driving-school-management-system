using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class LessonProgressRepository(ApplicationDbContext context) : ILessonProgressRepository
{
    public async Task<LessonProgress?> GetByIdAsync(
        int studentId, 
        int lessonId, 
        CancellationToken cancellationToken = default)
    {
        return await context.LessonProgresses
            .FirstOrDefaultAsync(
                lp => lp.StudentId == studentId && lp.LessonId == lessonId,
                cancellationToken);
    }

    public async Task<List<LessonProgress>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.LessonProgresses
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        LessonProgress lessonProgress, 
        CancellationToken cancellationToken = default)
    {
        await context.LessonProgresses.AddAsync(lessonProgress, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        LessonProgress lessonProgress, 
        CancellationToken cancellationToken = default)
    {
        context.LessonProgresses.Update(lessonProgress);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        LessonProgress lessonProgress, 
        CancellationToken cancellationToken = default)
    {
        context.LessonProgresses.Remove(lessonProgress);
        await context.SaveChangesAsync(cancellationToken);
    }
}