namespace DrivingSchool.Services.Contracts.Requests.Student;

public record CreateStudentRequest(
    string FirstName,
    string LastName,
    string Pesel,
    string PhoneNumber,
    string? Email,
    DateTime DateOfBirth
);