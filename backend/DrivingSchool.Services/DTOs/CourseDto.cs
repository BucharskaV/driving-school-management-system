namespace DrivingSchool.Services.DTOs;

public record CourseDto(
    int Id,
    string Title,
    decimal Price,
    List<LessonDto> Lessons
);