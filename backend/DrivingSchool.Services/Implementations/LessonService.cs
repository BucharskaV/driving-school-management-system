using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Implementations;

public class LessonService : ILessonService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly ICarRepository _carRepository;

    public LessonService(IStudentRepository studentRepository, ILessonRepository lessonRepository, ICarRepository carRepository)
    {
        _studentRepository = studentRepository;
        _lessonRepository = lessonRepository;
        _carRepository = carRepository;
    }

    public async Task<AvailabilityStatus> ValidateAvailabilityAsync(ValidateAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student is null)
            throw new StudentNotFoundException(request.StudentId);
        
        var type = await _lessonRepository.GetLessonTypeAsync(request.LessonId, cancellationToken);
        switch (type)
        {
            case LessonType.Theoretical when await _lessonRepository.IsLessonOfflineAsync(request.LessonId, cancellationToken):
            {
                var lesson = await _lessonRepository.GetTheoreticalLessonByIdAsync(request.LessonId, cancellationToken);
                if (lesson is null)
                    throw new LessonNotFoundException(request.LessonId);
                
                bool roomAvailable = lesson.RoomNumber != null && await _lessonRepository.IsRoomAvailableAsync(lesson.RoomNumber, request.Start, request.End, cancellationToken);
                if (!roomAvailable)
                    return AvailabilityStatus.RoomUnavailable;
                break;
            }
            case LessonType.Practical:
            {
                var lesson = await _lessonRepository.GetPracticalLessonByIdAsync(request.LessonId, cancellationToken);
                if (lesson is null)
                    throw new LessonNotFoundException(request.LessonId);
                
                bool carAvailable = await _carRepository.IsCarAvailableAsync(lesson.CarId, request.Start, request.End, cancellationToken);
                if (!carAvailable)
                    return AvailabilityStatus.CarUnavailable;
                break;
            }
        }

        return AvailabilityStatus.Available;
    }
}