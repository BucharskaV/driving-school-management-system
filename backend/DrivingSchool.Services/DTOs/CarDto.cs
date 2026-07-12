namespace DrivingSchool.Services.DTOs;

public record CarDto(
    int Id,
    string Brand,
    string Model,
    string RegistrationNumber
);