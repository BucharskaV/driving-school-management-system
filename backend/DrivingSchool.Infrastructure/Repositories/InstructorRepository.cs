using DrivingSchool.Domain.Enums;
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

    public async Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        return await context.LessonInstructors
            .Where(l => l.LessonId == lessonId)
            .Select(li => li.Instructor)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Instructor>> GetAvailableInstructorsByLessonIdAsync(DateTime start, DateTime end, int lessonId,
        CancellationToken cancellationToken = default)
    {
        return await context.LessonInstructors
            .Where(li => li.LessonId == lessonId)
            .Select(li => li.Instructor)
            .Where(i => !context.LessonProgresses.Any(lp =>
                lp.InstructorId == i.Id &&
                lp.ProgressStatus == ProgressStatus.Booked &&
                lp.StartTime < end &&
                lp.EndTime > start))
            .ToListAsync();
    }

    public async Task<Instructor?> GetRandomAvailableInstructorBySpecializationAsync(DateTime start, DateTime end, InstructorType instructorType,
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .OfType<Instructor>()
            .Where(i =>
                i.Specializations.Any(s => s.Type == instructorType) &&
                !context.LessonProgresses.Any(lp =>
                    lp.InstructorId == i.Id &&
                    lp.ProgressStatus == ProgressStatus.Booked &&
                    lp.StartTime < end &&
                    lp.EndTime > start))
            .OrderBy(_ => Guid.NewGuid())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsInstructorAvailableAsync(DateTime start, DateTime end, int instructorId,
        CancellationToken cancellationToken = default)
    {
        return !await context.LessonProgresses
            .AnyAsync(lp =>
                lp.InstructorId == instructorId &&
                lp.ProgressStatus == ProgressStatus.Booked &&
                lp.StartTime < end && lp.EndTime > start, cancellationToken);
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