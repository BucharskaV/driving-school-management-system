using DrivingSchool.Domain.Enums;
using DrivingSchool.Services.Contracts.Requests;

namespace DrivingSchool.Services.Interfaces;

public interface ILessonService
{
    Task<AvailabilityStatus> ValidateAvailabilityAsync(ValidateAvailabilityRequest request, CancellationToken cancellationToken);
}