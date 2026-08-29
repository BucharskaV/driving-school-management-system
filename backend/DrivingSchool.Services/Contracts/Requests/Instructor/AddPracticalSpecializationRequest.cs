namespace DrivingSchool.Services.Contracts.Requests.Instructor;

public record AddPracticalSpecializationRequest(
    string DrivingLicenseNumber,
    string MedicalCertificateNumber
    );