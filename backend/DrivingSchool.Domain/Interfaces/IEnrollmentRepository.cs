using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default);

    Task<List<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
}