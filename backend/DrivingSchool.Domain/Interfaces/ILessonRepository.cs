using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TheoreticalLesson?> GetTheoreticalLessonByIdAsync(int lessonId, CancellationToken cancellationToken = default);
    Task<PracticalLesson?> GetPracticalLessonByIdAsync(int lessonId, CancellationToken cancellationToken = default);
    Task<List<Lesson>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task UpdateAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task DeleteAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task<LessonType> GetLessonTypeAsync(int lessonId, CancellationToken cancellationToken = default);
    Task<bool> IsLessonOfflineAsync(int lessonId, CancellationToken cancellationToken = default);
    Task<bool> IsRoomAvailableAsync(string roomNumber, DateTime start, DateTime end, CancellationToken cancellationToken = default);
}