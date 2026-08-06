namespace DrivingSchool.Services.Contracts.Requests;

public record ValidateInstructorAvailabilityRequest(DateTime Start, DateTime End, int InstructorId);