using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Infrastructure.Repositories;

public class CourseRepository(ApplicationDbContext context) : ICourseRepository
{
    public async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Include(c => c.Category)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Include(c => c.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await context.Courses.AddAsync(course, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        context.Courses.Update(course);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Course course, CancellationToken cancellationToken = default)
    {
        context.Courses.Remove(course);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetCoursesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Where(c => c.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetEnrolledCoursesByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        return await context.Courses
            .Include(c => c.Lessons)
            .ThenInclude(l => l.LessonProgresses
                .Where(lp => lp.StudentId == studentId))
            .Include(c => c.Lessons)
            .ThenInclude(l => ((PracticalLesson)l).Car)
            .Include(c => c.Lessons)
            .ThenInclude(l => ((PracticalLesson)l).StartLocation)
            .Where(c => c.Enrollments.Any(e => e.StudentId == studentId))
            .ToListAsync(cancellationToken);
    }
}