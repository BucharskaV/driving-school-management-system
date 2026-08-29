using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Requests.Instructor;
using DrivingSchool.Services.Contracts.Responses;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface IInstructorService
{
    Task<List<Instructor>> GetInstructorsByLessonIdAsync(int lessonId, CancellationToken cancellationToken);
    Task<bool> ValidateAvailabilityAsync(ValidateInstructorAvailabilityRequest request, CancellationToken cancellationToken);
    Task<List<Instructor>> GetAvailableInstructorsAsync(GetAvailableInstructorsRequest request, CancellationToken cancellationToken);
    Task<SalaryInfoResponse> GetSalaryInfoAsync(int instructorId, CancellationToken cancellationToken);
    Task<List<InstructorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<InstructorDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<InstructorDto> CreateAsync(CreateInstructorRequest request, CancellationToken cancellationToken = default);
    Task<InstructorDto> UpdateAsync(int id, UpdateInstructorRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<List<InstructorSpecializationDto>> GetSpecializationsAsync(int instructorId, CancellationToken cancellationToken = default);
    Task<List<CertificationDto>> GetCertificationsAsync(int instructorId, CancellationToken cancellationToken = default);
    Task AddPracticalSpecializationAsync(int instructorId, AddPracticalSpecializationRequest request, CancellationToken cancellationToken = default);
    Task AddTheoreticalSpecializationAsync(int instructorId, CancellationToken cancellationToken = default);
    Task RemoveSpecializationAsync(int instructorId, InstructorType type, CancellationToken cancellationToken = default);
    Task<CertificationDto> AddCertificationAsync(int instructorId, AddCertificationRequest request, CancellationToken cancellationToken = default);
    Task RemoveCertificationAsync(int instructorId, int certificationId, CancellationToken cancellationToken = default);
}