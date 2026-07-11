using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<Course>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Course course, CancellationToken cancellationToken = default);

    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);

    Task DeleteAsync(Course course, CancellationToken cancellationToken = default);
}