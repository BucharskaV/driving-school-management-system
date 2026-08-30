using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public static class LessonProgressMapper
{
    public static LessonProgressDto MapToDto(LessonProgress progress)
    {
        return new LessonProgressDto(
            progress.StudentId,
            progress.LessonId,
            progress.ProgressStatus,
            progress.StartTime,
            progress.EndTime,
            progress.Note,
            progress.InstructorId,
            progress.ExtraFeeId);
    }
}