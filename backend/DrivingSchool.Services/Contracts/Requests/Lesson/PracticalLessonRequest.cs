namespace DrivingSchool.Services.Contracts.Requests.Lesson;

public record CreatePracticalLessonRequest(
    int CourseId,
    string Name,
    int SequenceNumber,
    TimeSpan Duration,
    int CarId,
    int StartLocationId
);

public record UpdatePracticalLessonRequest(
    int CourseId,
    string Name,
    int SequenceNumber,
    TimeSpan Duration,
    int CarId,
    int StartLocationId
);