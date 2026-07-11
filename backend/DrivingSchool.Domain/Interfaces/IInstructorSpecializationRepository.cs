using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IInstructorSpecializationRepository
{
    Task<InstructorSpecialization?> GetByIdAsync(int instructorId, InstructorType type, CancellationToken cancellationToken = default);
    Task<List<InstructorSpecialization>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(InstructorSpecialization instructorSpecialization, CancellationToken cancellationToken = default);
    Task UpdateAsync(InstructorSpecialization instructorSpecialization, CancellationToken cancellationToken = default);
    Task DeleteAsync(InstructorSpecialization instructorSpecialization, CancellationToken cancellationToken = default);
}