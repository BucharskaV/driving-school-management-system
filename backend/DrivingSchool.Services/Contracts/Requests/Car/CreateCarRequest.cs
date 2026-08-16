namespace DrivingSchool.Services.Contracts.Requests.Car;

public record CreateCarRequest(
    string Brand,
    string Model,
    string RegistrationNumber
);