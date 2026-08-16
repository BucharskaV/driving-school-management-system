using DrivingSchool.Services.Contracts.Requests.Student;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface IStudentService
{
    Task<List<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StudentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StudentDto> CreateAsync(CreateStudentRequest request, CancellationToken cancellationToken = default);
    Task<StudentDto> UpdateAsync(int id, UpdateStudentRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}