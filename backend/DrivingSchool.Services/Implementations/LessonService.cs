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
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IInstructorRepository _instructorRepository;

    public LessonService(IStudentRepository studentRepository, ILessonRepository lessonRepository, 
        ICarRepository carRepository, ILessonProgressRepository lessonProgressRepository,
        IInstructorRepository instructorRepository)
    {
        _studentRepository = studentRepository;
        _lessonRepository = lessonRepository;
        _carRepository = carRepository;
        _lessonProgressRepository = lessonProgressRepository;
        _instructorRepository = instructorRepository;
    }

    public async Task<AvailabilityStatus> ValidateAvailabilityAsync(ValidateAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
        if (student is null)
            throw new StudentNotFoundException(request.StudentId);
        if(!await _studentRepository.IsStudentAvailable(request.StudentId, request.Start, request.End, cancellationToken))
            return AvailabilityStatus.StudentUnavailable;
        
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
    
    public async Task BookLessonAsync(BookLessonRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);
        
        var progress = await _lessonProgressRepository.GetByIdAsync(request.StudentId, request.LessonId, cancellationToken);
        if (progress == null)
            throw new ProgressNotFoundException();
        
        var instructor = await _instructorRepository.GetByIdAsync(request.InstructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();
         
        progress.ProgressStatus = ProgressStatus.Booked;
        progress.StartTime = request.StartTime;
        progress.EndTime = request.EndTime;
        progress.Instructor = instructor;
        progress.InstructorId = instructor.Id;
        
        await _lessonProgressRepository.UpdateAsync(progress, cancellationToken);
    }
    
    private async Task ValidateAsync(BookLessonRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new InvalidRequestException();
        if (request.StartTime == null || request.EndTime == null)
            throw new TimeIsRequiredException();
        
        var req = new ValidateAvailabilityRequest(request.StudentId, request.LessonId, request.StartTime, request.EndTime);
        var availability = await ValidateAvailabilityAsync(req, cancellationToken);

        if (availability != AvailabilityStatus.Available)
        {
            throw availability switch
            {
                AvailabilityStatus.StudentUnavailable =>
                    new StudentUnavailableException(),

                AvailabilityStatus.CarUnavailable =>
                    new CarUnavailableException(),

                AvailabilityStatus.RoomUnavailable =>
                    new RoomUnavailableException(),

                _ => new InvalidOperationException("Unknown availability status.")
            };
        }
        
        IsInOfficeHours(request.StartTime, request.EndTime);
    }

    private void IsInOfficeHours(DateTime start, DateTime end)
    {
        var startLocal = start.ToLocalTime();
        var endLocal = end.ToLocalTime();
        if (startLocal < DateTime.Now)
            throw new StartTimeIsInPastException();
        
        var openingTime = TimeSpan.FromHours(8);  
        var closingTime = TimeSpan.FromHours(20);

        if(startLocal.TimeOfDay <= openingTime || endLocal.TimeOfDay >= closingTime)
            throw new OfficeHoursException(openingTime, closingTime);
    }
}