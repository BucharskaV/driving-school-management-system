using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Student;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<List<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var students = await _studentRepository.GetAllAsync(cancellationToken);

        return students
            .Select(MapToDto)
            .ToList();
    }

    public async Task<StudentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(id);

        return MapToDto(student);
    }

    public async Task<StudentDto> CreateAsync(CreateStudentRequest request, CancellationToken cancellationToken = default)
    {
        var student = new Student(
            request.FirstName,
            request.LastName,
            request.Pesel,
            Role.Student,
            request.PhoneNumber,
            request.Email,
            request.DateOfBirth);

        await _studentRepository.AddAsync(student, cancellationToken);

        return MapToDto(student);
    }

    public async Task<StudentDto> UpdateAsync(int id, UpdateStudentRequest request, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(id);

        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.PhoneNumber = request.PhoneNumber;
        student.Email = request.Email;
        student.DateOfBirth = request.DateOfBirth;

        await _studentRepository.UpdateAsync(student, cancellationToken);

        return MapToDto(student);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(id);

        await _studentRepository.DeleteAsync(student, cancellationToken);
    }

    private static StudentDto MapToDto(Student student)
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
}