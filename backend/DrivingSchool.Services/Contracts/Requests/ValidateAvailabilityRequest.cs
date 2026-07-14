namespace DrivingSchool.Services.Contracts.Requests;

public record ValidateAvailabilityRequest(int StudentId, int LessonId, DateTime Start, DateTime End);