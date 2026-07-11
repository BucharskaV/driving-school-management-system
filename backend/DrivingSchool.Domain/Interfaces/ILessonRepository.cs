using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Lesson>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task UpdateAsync(Lesson lesson, CancellationToken cancellationToken = default);
    Task DeleteAsync(Lesson lesson, CancellationToken cancellationToken = default);
}