namespace DrivingSchool.Services.Contracts.Requests;

public record GetAvailableInstructorsRequest(DateTime Start, DateTime End, int LessonId);