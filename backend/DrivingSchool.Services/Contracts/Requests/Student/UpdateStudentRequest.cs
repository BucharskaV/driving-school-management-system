namespace DrivingSchool.Services.Contracts.Requests.Student;

public record UpdateStudentRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? Email,
    DateTime DateOfBirth
);