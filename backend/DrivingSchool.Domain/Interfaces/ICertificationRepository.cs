using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ICertificationRepository
{
    Task<Certification?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Certification>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Certification certification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Certification certification, CancellationToken cancellationToken = default);
    Task DeleteAsync(Certification certification, CancellationToken cancellationToken = default);
}