using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Services.DTOs;

public record BookingDto(
    int StudentId,
    string StudentName,
    int LessonId,
    string LessonName,
    int SequenceNumber,
    DateTime? StartTime,
    DateTime? EndTime,
    ProgressStatus Status,
    string? Note,
    int? InstructorId,
    string? InstructorName,
    int? ExtraFeeId
);