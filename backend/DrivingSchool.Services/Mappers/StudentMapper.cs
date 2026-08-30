using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Student;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Mappers;

public static class StudentMapper
{
    public static StudentDto MapToDto(Student student)
    {
        return new StudentDto(
            student.Id,
            student.FirstName,
            student.LastName,
            student.Pesel,
            student.PhoneNumber,
            student.Email,
            student.DateOfBirth,
            student.Age);
    }
    
    public static Student MapToEntity(CreateStudentRequest request)
    {
        return new Student(
            request.FirstName,
            request.LastName,
            request.Pesel,
            Role.Student,
            request.PhoneNumber,
            request.Email,
            request.DateOfBirth);
    }
}