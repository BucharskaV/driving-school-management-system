using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Address>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Address address, CancellationToken cancellationToken = default);
    Task UpdateAsync(Address address, CancellationToken cancellationToken = default);
    Task DeleteAsync(Address address, CancellationToken cancellationToken = default);
}