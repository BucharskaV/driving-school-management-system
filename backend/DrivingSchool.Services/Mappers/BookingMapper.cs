using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public static class BookingMapper
{
    public static BookingDto MapToDto(LessonProgress progress)
    {
        return new BookingDto(
            progress.StudentId,
            progress.Student.FullName,
            progress.LessonId,
            progress.Lesson.Name,
            progress.Lesson.SequenceNumber,
            progress.StartTime,
            progress.EndTime,
            progress.ProgressStatus,
            progress.Note,
            progress.InstructorId,
            progress.Instructor?.FullName,
            progress.ExtraFeeId
        );
    }
}