using DrivingSchool.Domain.Enums;
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

    public async Task<TheoreticalLesson?> GetTheoreticalLessonByIdAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .OfType<TheoreticalLesson>()
            .FirstOrDefaultAsync(l => l.Id == lessonId, cancellationToken);
    }

    public async Task<PracticalLesson?> GetPracticalLessonByIdAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .OfType<PracticalLesson>()
            .Include(l => l.Car)
            .Include(l => l.StartLocation)
            .FirstOrDefaultAsync(l => l.Id == lessonId, cancellationToken);
    }

    public async Task<List<Lesson>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Lesson>> GetLessonsWithProgressByInstructorIdAsync(int instructorId, CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .Include(l => l.LessonProgresses
                .Where(lp => lp.InstructorId == instructorId))
            .Where(l => l.LessonProgresses.Any(lp => lp.InstructorId == instructorId))
            .OrderBy(l => l.LessonProgresses
                .Where(lp => lp.InstructorId == instructorId)
                .Select(lp => lp.StartTime)
                .FirstOrDefault())
            .Include(l => (l as PracticalLesson).Car)
            .Include(l => (l as PracticalLesson).StartLocation)
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

    public async Task<LessonType> GetLessonTypeAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId, cancellationToken);
        return lesson switch
        {
            PracticalLesson => LessonType.Practical,
            TheoreticalLesson => LessonType.Theoretical,
            _ => throw new Exception("Unknown")
        };
    }

    public async Task<bool> IsLessonOfflineAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        return await context.Lessons
            .OfType<TheoreticalLesson>()
            .Where(l => l.Id == lessonId)
            .Select(l => !l.IsOnline)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsRoomAvailableAsync(string roomNumber, DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        return !await context.LessonProgresses
            .Where(lp => lp.Lesson is TheoreticalLesson)
            .AnyAsync(lp =>
                ((TheoreticalLesson)lp.Lesson).RoomNumber == roomNumber &&
                lp.ProgressStatus == ProgressStatus.Booked &&
                lp.StartTime < end && lp.EndTime > start,
                cancellationToken);
    }
}