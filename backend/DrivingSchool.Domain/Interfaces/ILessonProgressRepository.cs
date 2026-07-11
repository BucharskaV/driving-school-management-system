using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ILessonProgressRepository
{
    Task<LessonProgress?> GetByIdAsync(int studentId, int lessonId, CancellationToken cancellationToken = default);
    Task<List<LessonProgress>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(LessonProgress lessonProgress, CancellationToken cancellationToken = default);
    Task UpdateAsync(LessonProgress lessonProgress, CancellationToken cancellationToken = default);
    Task DeleteAsync(LessonProgress lessonProgress, CancellationToken cancellationToken = default);
}