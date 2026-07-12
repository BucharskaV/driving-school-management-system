namespace DrivingSchool.Services.DTOs;

public record EnrolledCourseDto(
    int Id,
    string Title,
    decimal Price,
    IEnumerable<LessonDto> Lessons
);