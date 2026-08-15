using DrivingSchool.Domain.Enums;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface ILessonService
{
    Task<AvailabilityStatus> ValidateAvailabilityAsync(ValidateAvailabilityRequest request, CancellationToken cancellationToken);
    Task BookLessonAsync(BookLessonRequest request, CancellationToken cancellationToken);
    Task<List<LessonDto>> GetLessonsWithProgressByInstructorIdAsync(int instructorId, CancellationToken cancellationToken);
    Task AddNoteToLessonAsync(int studentId, int lessonId, string input, CancellationToken cancellationToken);
    Task ChangeBookingStatusAsync(int studentId, int lessonId, ProgressStatus status, CancellationToken cancellationToken);
}