using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Student student, CancellationToken cancellationToken = default);
    Task UpdateAsync(Student student, CancellationToken cancellationToken = default);
    Task DeleteAsync(Student student, CancellationToken cancellationToken = default);
    Task<bool> IsStudentAvailable(int studentId, DateTime start, DateTime end, CancellationToken cancellationToken = default);
}