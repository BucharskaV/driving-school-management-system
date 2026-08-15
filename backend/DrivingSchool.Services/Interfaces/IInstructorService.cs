using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Responses;

namespace DrivingSchool.Services.Interfaces;

public interface IInstructorService
{
    Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken);
    Task<bool> ValidateAvailabilityAsync(ValidateInstructorAvailabilityRequest request, CancellationToken cancellationToken);
    Task<List<Instructor>> GetAvailableInstructorsAsync(GetAvailableInstructorsRequest request, CancellationToken cancellationToken);
    Task<SalaryInfoResponse> GetSalaryInfoAsync(int instructorId, CancellationToken cancellationToken);
}