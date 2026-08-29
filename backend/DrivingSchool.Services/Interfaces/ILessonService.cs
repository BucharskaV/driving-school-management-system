using DrivingSchool.Domain.Enums;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Requests.Lesson;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface ILessonService
{
    Task<List<LessonDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LessonDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<LessonDto> CreatePracticalAsync(CreatePracticalLessonRequest request, CancellationToken cancellationToken = default);
    Task<LessonDto> CreateTheoreticalAsync(CreateTheoreticalLessonRequest request, CancellationToken cancellationToken = default);
    Task<LessonDto> UpdatePracticalAsync(int id, UpdatePracticalLessonRequest request, CancellationToken cancellationToken = default);
    Task<LessonDto> UpdateTheoreticalAsync(int id, UpdateTheoreticalLessonRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<AvailabilityStatus> ValidateAvailabilityAsync(ValidateAvailabilityRequest request, CancellationToken cancellationToken);
    Task BookLessonAsync(BookLessonRequest request, CancellationToken cancellationToken);
    Task<List<LessonDto>> GetLessonsWithProgressByInstructorIdAsync(int instructorId, CancellationToken cancellationToken);
    Task AddNoteToLessonAsync(int studentId, int lessonId, string input, CancellationToken cancellationToken);
    Task ChangeBookingStatusAsync(int studentId, int lessonId, ProgressStatus status, CancellationToken cancellationToken);
    Task<List<BookingDto>> GetBookingsByInstructorIdAsync(int instructorId, CancellationToken cancellationToken = default);
    Task<List<BookingDto>> GetBookingsByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
}