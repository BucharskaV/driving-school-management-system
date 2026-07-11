using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IInstructorRepository
{
    Task<Instructor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Instructor?> GetByCodeAsync(string instructorCode, CancellationToken cancellationToken = default);
    Task<List<Instructor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default);
    Task UpdateAsync(Instructor instructor, CancellationToken cancellationToken = default);
    Task DeleteAsync(Instructor instructor, CancellationToken cancellationToken = default);
}