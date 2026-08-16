using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Services.DTOs;

public record InstructorDto(
    int Id,
    string FirstName,
    string LastName,
    string Pesel,
    string? Email,
    string PhoneNumber,
    string InstructorCode,
    decimal BaseSalary,
    decimal? Bonus,
    decimal TotalSalary,
    string? DrivingLicenseNumber,
    string? MedicalCertificateNumber,
    List<InstructorType> Specializations
);