using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Services.DTOs;

public record LessonProgressDto(
    int StudentId,
    int LessonId,
    ProgressStatus ProgressStatus,
    DateTime? StartTime,
    DateTime? EndTime,
    string? Note,
    int? InstructorId,
    int? ExtraFeeId
);