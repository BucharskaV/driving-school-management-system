using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface ICarRepository
{
    Task<Car?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Car car, CancellationToken cancellationToken = default);
    Task UpdateAsync(Car car, CancellationToken cancellationToken = default);
    Task DeleteAsync(Car car, CancellationToken cancellationToken = default);
}