using DrivingSchool.Domain.Models;

namespace DrivingSchool.Domain.Interfaces;

public interface IExtraFeeRepository
{
    Task<ExtraFee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<ExtraFee>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(ExtraFee extraFee, CancellationToken cancellationToken = default);

    Task UpdateAsync(ExtraFee extraFee, CancellationToken cancellationToken = default);

    Task DeleteAsync(ExtraFee extraFee, CancellationToken cancellationToken = default);
}