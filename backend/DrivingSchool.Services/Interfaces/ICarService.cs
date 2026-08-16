using DrivingSchool.Services.Contracts.Requests.Car;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface ICarService
{
    Task<List<CarDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CarDto> CreateAsync(CreateCarRequest request, CancellationToken cancellationToken = default);
    Task<CarDto> UpdateAsync(int id, UpdateCarRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}