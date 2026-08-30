using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public static class LessonMapper
{
    public static LessonDto MapToDto(Lesson lesson)
    {
        var practical = lesson as PracticalLesson;
        var theoretical = lesson as TheoreticalLesson;

        var progress = lesson.LessonProgresses.FirstOrDefault();

        return new LessonDto(
            lesson.Id,
            lesson.Name,
            lesson.SequenceNumber,
            lesson.Duration,

            practical?.Car is null
                ? null
                : CarMapper.MapToDto(practical.Car),

            practical?.StartLocation is null
                ? null
                : AddressMapper.MapToDto(practical.StartLocation),

            theoretical?.Topic,
            theoretical?.RoomNumber,
            theoretical?.IsOnline,

            progress is null
                ? null
                : LessonProgressMapper.MapToDto(progress)
        );
    }
}