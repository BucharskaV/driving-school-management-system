namespace DrivingSchool.Services.DTOs;

public record LessonDto(
    int Id,
    string Name,
    int SequenceNumber,
    TimeSpan Duration,
    CarDto? Car,
    AddressDto? StartLocation,
    string? Topic,
    string? RoomNumber,
    bool? IsOnline,
    LessonProgressDto? Progress
);