using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ILessonInstructorRepository
{
    Task<LessonInstructor?> GetByIdAsync(int lessonId, int instructorId, CancellationToken cancellationToken = default);
    Task<List<LessonInstructor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(LessonInstructor lessonInstructor, CancellationToken cancellationToken = default);
    Task UpdateAsync(LessonInstructor lessonInstructor, CancellationToken cancellationToken = default);
    Task DeleteAsync(LessonInstructor lessonInstructor, CancellationToken cancellationToken = default);
}