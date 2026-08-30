using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Requests.Instructor;
using DrivingSchool.Services.Contracts.Responses;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

namespace DrivingSchool.Services.Implementations;

public class InstructorService : IInstructorService
{
    private readonly IInstructorRepository _instructorRepository;
    private readonly ILessonRepository _lessonRepository;

    public InstructorService(IInstructorRepository instructorRepository, ILessonRepository lessonRepository)
    {
        _instructorRepository = instructorRepository;
        _lessonRepository = lessonRepository;
    }

    public async Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken = default)
    {
        return await _instructorRepository.GetInstructorsByLessonIdAsync(lessonId, cancellationToken);
    }

    public async Task<bool> ValidateAvailabilityAsync(ValidateInstructorAvailabilityRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _instructorRepository.IsInstructorAvailableAsync(
            request.Start,
            request.End,
            request.InstructorId,
            cancellationToken);
    }

    public async Task<List<Instructor>> GetAvailableInstructorsAsync(GetAvailableInstructorsRequest request,
        CancellationToken cancellationToken = default)
    {
        var instructors = await _instructorRepository.GetAvailableInstructorsByLessonIdAsync(
            request.Start,
            request.End,
            request.LessonId,
            cancellationToken);

        if (instructors.Count == 0)
        {
            var type = await _lessonRepository.GetLessonTypeAsync(
                request.LessonId,
                cancellationToken);

            var specialization = type switch
            {
                LessonType.Theoretical => InstructorType.TheoreticalInstructor,
                LessonType.Practical => InstructorType.PracticalInstructor,
                _ => throw new SpecializationNotFoundException()
            };

            var randomInstructor =
                await _instructorRepository.GetRandomAvailableInstructorBySpecializationAsync(
                    request.Start,
                    request.End,
                    specialization,
                    cancellationToken);

            if (randomInstructor != null)
                instructors.Add(randomInstructor);
        }

        return instructors;
    }

    public async Task<SalaryInfoResponse> GetSalaryInfoAsync(int instructorId, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();
        
        return new SalaryInfoResponse(instructor.BaseSalary, instructor.Bonus, instructor.TotalSalary);
    }

    public async Task<List<InstructorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var instructors = await _instructorRepository.GetAllAsync(cancellationToken);

        return instructors
            .Select(InstructorMapper.MapToDto)
            .ToList();
    }

    public async Task<InstructorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(id, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        return InstructorMapper.MapToDto(instructor);
    }

    public async Task<InstructorDto> CreateAsync(CreateInstructorRequest request, CancellationToken cancellationToken = default)
    {
        var instructor = InstructorMapper.MapToEntity(request);

        await _instructorRepository.AddAsync(instructor, cancellationToken);

        return InstructorMapper.MapToDto(instructor);
    }

    public async Task<InstructorDto> UpdateAsync(int id, UpdateInstructorRequest request, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(id, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        instructor.FirstName = request.FirstName;
        instructor.LastName = request.LastName;
        instructor.Email = request.Email;
        instructor.PhoneNumber = request.PhoneNumber;
        instructor.BaseSalary = request.BaseSalary;
        instructor.Bonus = request.Bonus;
        instructor.DrivingLicenseNumber = request.DrivingLicenseNumber;
        instructor.MedicalCertificateNumber = request.MedicalCertificateNumber;

        await _instructorRepository
            .UpdateAsync(instructor, cancellationToken);

        return InstructorMapper.MapToDto(instructor);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(id, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        await _instructorRepository
            .DeleteAsync(instructor, cancellationToken);
    }
    
    public async Task<List<InstructorSpecializationDto>> GetSpecializationsAsync(int instructorId, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        return instructor.Specializations
            .Select(s => new InstructorSpecializationDto(s.Type))
            .ToList();
    }
    
    public async Task<List<CertificationDto>> GetCertificationsAsync(int instructorId, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        return instructor.Certifications
            .Select(InstructorMapper.MapCertificationToDto)
            .ToList();
    }
    
    public async Task AddPracticalSpecializationAsync(int instructorId, AddPracticalSpecializationRequest request, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);

        if (instructor == null)
            throw new InstructorNotFoundException();

        instructor.AddPracticalSpecialization(request.DrivingLicenseNumber, request.MedicalCertificateNumber);
        await _instructorRepository.UpdateAsync(instructor, cancellationToken);
    }
    
    public async Task AddTheoreticalSpecializationAsync(int instructorId, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        instructor.AddTheoreticalSpecialization();
        await _instructorRepository.UpdateAsync(instructor, cancellationToken);
    }
    
    public async Task RemoveSpecializationAsync(int instructorId, InstructorType type, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        instructor.RemoveSpecialization(type);
        await _instructorRepository.UpdateAsync(instructor, cancellationToken);
    }
    
    public async Task<CertificationDto> AddCertificationAsync(int instructorId, AddCertificationRequest request, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        var certification = instructor.AddCertification(request.Description);

        await _instructorRepository.UpdateAsync(instructor, cancellationToken);

        return new CertificationDto(certification.Id, certification.Description);
    }
    
    public async Task RemoveCertificationAsync(int instructorId, int certificationId, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        instructor.RemoveCertification(certificationId);

        await _instructorRepository.UpdateAsync(instructor, cancellationToken);
    }
}