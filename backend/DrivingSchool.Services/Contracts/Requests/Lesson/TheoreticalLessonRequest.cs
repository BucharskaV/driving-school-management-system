namespace DrivingSchool.Services.Contracts.Requests.Lesson;

public record CreateTheoreticalLessonRequest(
    int CourseId,
    string Name,
    int SequenceNumber,
    TimeSpan Duration,
    string Topic,
    string RoomNumber,
    bool IsOnline
);

public record UpdateTheoreticalLessonRequest(
    int CourseId,
    string Name,
    int SequenceNumber,
    TimeSpan Duration,
    string Topic,
    string RoomNumber,
    bool IsOnline
);