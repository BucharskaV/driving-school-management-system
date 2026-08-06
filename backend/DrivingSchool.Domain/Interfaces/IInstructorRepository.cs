using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IInstructorRepository
{
    Task<Instructor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Instructor?> GetByCodeAsync(string instructorCode, CancellationToken cancellationToken = default);
    Task<List<Instructor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken = default);
    Task<List<Instructor>> GetAvailableInstructorsByLessonIdAsync(DateTime start, DateTime end, int lessonId, CancellationToken cancellationToken = default);
    Task<Instructor?> GetRandomAvailableInstructorBySpecializationAsync(DateTime start, DateTime end, InstructorType instructorType, CancellationToken cancellationToken = default);
    Task<bool> IsInstructorAvailableAsync(DateTime start, DateTime end, int instructorId, CancellationToken cancellationToken = default);
    Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default);
    Task UpdateAsync(Instructor instructor, CancellationToken cancellationToken = default);
    Task DeleteAsync(Instructor instructor, CancellationToken cancellationToken = default);
}