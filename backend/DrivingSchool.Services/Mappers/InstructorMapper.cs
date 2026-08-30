using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Instructor;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public static class InstructorMapper
{
    public static InstructorDto MapToDto(Instructor instructor)
    {
        return new InstructorDto(
            instructor.Id,
            instructor.FirstName,
            instructor.LastName,
            instructor.Pesel,
            instructor.Email,
            instructor.PhoneNumber,
            instructor.InstructorCode,
            instructor.BaseSalary,
            instructor.Bonus,
            instructor.TotalSalary,
            instructor.DrivingLicenseNumber,
            instructor.MedicalCertificateNumber,
            instructor.Specializations
                .Select(s => s.Type)
                .ToList());
    }
    
    public static Instructor MapToEntity(CreateInstructorRequest request)
    {
        return new Instructor(
            request.Specializations,
            request.FirstName,
            request.LastName,
            Role.Instructor,
            request.Pesel,
            request.PhoneNumber,
            request.Email,
            request.InstructorCode,
            request.BaseSalary,
            request.Bonus,
            request.DrivingLicenseNumber,
            request.MedicalCertificateNumber);
    }
    
    public static CertificationDto MapCertificationToDto(Certification c)
    {
        return new CertificationDto(c.Id, c.Description);
    }
}