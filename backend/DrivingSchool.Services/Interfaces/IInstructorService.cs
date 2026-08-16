using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Requests.Instructor;
using DrivingSchool.Services.Contracts.Responses;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface IInstructorService
{
    Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken);
    Task<bool> ValidateAvailabilityAsync(ValidateInstructorAvailabilityRequest request, CancellationToken cancellationToken);
    Task<List<Instructor>> GetAvailableInstructorsAsync(GetAvailableInstructorsRequest request, CancellationToken cancellationToken);
    Task<SalaryInfoResponse> GetSalaryInfoAsync(int instructorId, CancellationToken cancellationToken);
    Task<List<InstructorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<InstructorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<InstructorDto> CreateAsync(CreateInstructorRequest request, CancellationToken cancellationToken = default);
    Task<InstructorDto> UpdateAsync(int id, UpdateInstructorRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}