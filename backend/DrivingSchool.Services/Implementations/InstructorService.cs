using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Responses;
using DrivingSchool.Services.Interfaces;

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
}