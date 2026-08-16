using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Services.Contracts.Requests.Instructor;

public record CreateInstructorRequest(
    string FirstName,
    string LastName,
    string Pesel,
    string? Email,
    string PhoneNumber,
    string InstructorCode,
    decimal BaseSalary,
    decimal? Bonus,
    List<InstructorType> Specializations,
    string? DrivingLicenseNumber,
    string? MedicalCertificateNumber
);