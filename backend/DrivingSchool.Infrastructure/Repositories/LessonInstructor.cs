using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class LessonInstructorRepository(ApplicationDbContext context) : ILessonInstructorRepository
{
    public async Task<LessonInstructor?> GetByIdAsync(int lessonId, int instructorId, CancellationToken cancellationToken = default)
    {
        return await context.LessonInstructors
            .FirstOrDefaultAsync(
                x => x.LessonId == lessonId && x.InstructorId == instructorId,
                cancellationToken);
    }

    public async Task<List<LessonInstructor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.LessonInstructors.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        LessonInstructor lessonInstructor, 
        CancellationToken cancellationToken = default)
    {
        await context.LessonInstructors.AddAsync(lessonInstructor, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        LessonInstructor lessonInstructor, 
        CancellationToken cancellationToken = default)
    {
        context.LessonInstructors.Update(lessonInstructor);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        LessonInstructor lessonInstructor, 
        CancellationToken cancellationToken = default)
    {
        context.LessonInstructors.Remove(lessonInstructor);
        await context.SaveChangesAsync(cancellationToken);
    }
}