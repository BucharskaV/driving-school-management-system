using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;

namespace DrivingSchool.Services.Interfaces;

public interface IInstructorService
{
    Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken = default);
    Task<bool> ValidateAvailabilityAsync(ValidateInstructorAvailabilityRequest request, CancellationToken cancellationToken = default);
    Task<List<Instructor>> GetAvailableInstructorsAsync(GetAvailableInstructorsRequest request, CancellationToken cancellationToken = default);
}