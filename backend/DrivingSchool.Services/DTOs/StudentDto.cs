namespace DrivingSchool.Services.DTOs;

public record StudentDto(
    int Id,
    string FirstName,
    string LastName,
    string Pesel,
    string PhoneNumber,
    string? Email,
    DateTime DateOfBirth,
    int Age
);