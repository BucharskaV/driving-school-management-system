using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Student;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

namespace DrivingSchool.Services.Implementations;

public class StudentService(IStudentRepository studentRepository) : IStudentService
{
    public async Task<List<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var students = await studentRepository.GetAllAsync(cancellationToken);

        return students
            .Select(StudentMapper.MapToDto)
            .ToList();
    }

    public async Task<StudentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetByIdAsync(id, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(id);

        return StudentMapper.MapToDto(student);
    }

    public async Task<StudentDto> CreateAsync(CreateStudentRequest request, CancellationToken cancellationToken = default)
    {
        var student = StudentMapper.MapToEntity(request);

        await studentRepository.AddAsync(student, cancellationToken);

        return StudentMapper.MapToDto(student);
    }

    public async Task<StudentDto> UpdateAsync(int id, UpdateStudentRequest request, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetByIdAsync(id, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(id);

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.PhoneNumber = request.PhoneNumber;
        student.Email = request.Email;
        student.DateOfBirth = request.DateOfBirth;

        await studentRepository.UpdateAsync(student, cancellationToken);

        return StudentMapper.MapToDto(student);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await studentRepository.GetByIdAsync(id, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(id);

        await studentRepository.DeleteAsync(student, cancellationToken);
    }
}