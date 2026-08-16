namespace DrivingSchool.Services.Contracts.Requests.Instructor;

public record UpdateInstructorRequest(
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber,
    decimal BaseSalary,
    decimal? Bonus,
    string? DrivingLicenseNumber,
    string? MedicalCertificateNumber
);