using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests;
using DrivingSchool.Services.Contracts.Requests.Lesson;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

namespace DrivingSchool.Services.Implementations;

public class LessonService : ILessonService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly ICarRepository _carRepository;
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ITimeService _timeService;

    public LessonService(IStudentRepository studentRepository, ILessonRepository lessonRepository, 
        ICarRepository carRepository, ILessonProgressRepository lessonProgressRepository,
        IInstructorRepository instructorRepository, IAddressRepository addressRepository,
        ICourseRepository courseRepository, ITimeService timeService)
    {
        _studentRepository = studentRepository;
        _lessonRepository = lessonRepository;
        _carRepository = carRepository;
        _lessonProgressRepository = lessonProgressRepository;
        _instructorRepository = instructorRepository;
        _addressRepository = addressRepository;
        _courseRepository = courseRepository;
        _timeService = timeService;
    }
    
    public async Task<List<LessonDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lessons = await _lessonRepository.GetAllAsync(cancellationToken);

        return lessons
            .Select(LessonMapper.MapToDto)
            .ToList();
    }

    public async Task<LessonDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id, cancellationToken);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        return LessonMapper.MapToDto(lesson);
    }

    public async Task<LessonDto> CreatePracticalAsync(CreatePracticalLessonRequest request, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
            throw new CourseNotFoundException(request.CourseId);
        
        var car = await _carRepository.GetByIdAsync(request.CarId, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(request.CarId);

        var address = await _addressRepository.GetByIdAsync(request.StartLocationId, cancellationToken);

        if (address == null)
            throw new AddressNotFoundException(request.StartLocationId);

        var lesson = new PracticalLesson(
            course,
            car,
            request.Name,
            request.SequenceNumber,
            request.Duration,
            address);

        await _lessonRepository.AddAsync(lesson, cancellationToken);

        return LessonMapper.MapToDto(lesson);
    }

    public async Task<LessonDto> CreateTheoreticalAsync(
        CreateTheoreticalLessonRequest request,
        CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
            throw new CourseNotFoundException(request.CourseId);
        
        var lesson = new TheoreticalLesson(
            course,
            request.Name,
            request.SequenceNumber,
            request.Duration,
            request.Topic,
            request.IsOnline,
            request.RoomNumber);

        await _lessonRepository.AddAsync(lesson, cancellationToken);

        return LessonMapper.MapToDto(lesson);
    }

    public async Task<LessonDto> UpdatePracticalAsync(int id, UpdatePracticalLessonRequest request, CancellationToken cancellationToken = default)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id, cancellationToken);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        if (lesson is not PracticalLesson practicalLesson) 
            throw new InvalidOperationException("The specified lesson is not a practical lesson.");

        var car = await _carRepository.GetByIdAsync(request.CarId, cancellationToken);
        if (car == null)
            throw new CarNotFoundException(request.CarId);

        var address = await _addressRepository.GetByIdAsync(request.StartLocationId, cancellationToken);
        if (address == null)
            throw new AddressNotFoundException(request.StartLocationId);

        practicalLesson.Name = request.Name;
        practicalLesson.SequenceNumber = request.SequenceNumber;
        practicalLesson.Duration = request.Duration;
        practicalLesson.Car = car;
        practicalLesson.StartLocation = address;

        await _lessonRepository.UpdateAsync(practicalLesson, cancellationToken);

        return LessonMapper.MapToDto(practicalLesson);
    }

    public async Task<LessonDto> UpdateTheoreticalAsync(int id, UpdateTheoreticalLessonRequest request, CancellationToken cancellationToken = default)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id, cancellationToken);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        if (lesson is not TheoreticalLesson theoreticalLesson)
            throw new InvalidOperationException("The specified lesson is not a theoretical lesson.");

        theoreticalLesson.Name = request.Name;
        theoreticalLesson.SequenceNumber = request.SequenceNumber;
        theoreticalLesson.Duration = request.Duration;
        theoreticalLesson.Topic = request.Topic;
        theoreticalLesson.RoomNumber = request.RoomNumber;
        theoreticalLesson.IsOnline = request.IsOnline;

        await _lessonRepository.UpdateAsync(theoreticalLesson, cancellationToken);

        return LessonMapper.MapToDto(theoreticalLesson);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id, cancellationToken);
        if (lesson == null)
            throw new LessonNotFoundException(id);

        await _lessonRepository.DeleteAsync(lesson, cancellationToken);
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

    public async Task<List<LessonDto>> GetLessonsWithProgressByInstructorIdAsync(int instructorId, CancellationToken cancellationToken)
    {
        var lessons = await _lessonRepository.GetLessonsWithProgressByInstructorIdAsync(instructorId, cancellationToken);
        
        return lessons
            .Select(LessonMapper.MapToDto)
            .ToList();
    }

    public async Task AddNoteToLessonAsync(int studentId, int lessonId, string input, CancellationToken cancellationToken)
    {
        var progress = await _lessonProgressRepository.GetByIdAsync(studentId, lessonId, cancellationToken);
        if (progress == null)
            throw new ProgressNotFoundException();
        
        var newNote = progress.Note + $"{Environment.NewLine}"+ input;
        if(newNote.Length > 200)
            throw new InvalidNoteException();
        
        progress.Note = newNote;
        await _lessonProgressRepository.UpdateAsync(progress, cancellationToken);
    }

    public async Task ChangeBookingStatusAsync(int studentId, int lessonId, ProgressStatus status, CancellationToken cancellationToken)
    {
        var progress = await _lessonProgressRepository.GetByIdAsync(studentId, lessonId, cancellationToken);
        if (progress == null)
            throw new ProgressNotFoundException();

        if (status == ProgressStatus.Locked)
            throw new PermissionDeniedLockedStatusException();
         
        progress.ProgressStatus = status;
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
        
        _timeService.IsWithinOfficeHours(request.StartTime, request.EndTime);
    }
    
    public async Task<List<BookingDto>> GetBookingsByInstructorIdAsync(int instructorId, CancellationToken cancellationToken = default)
    {
        var instructor = await _instructorRepository.GetByIdAsync(instructorId, cancellationToken);
        if (instructor == null)
            throw new InstructorNotFoundException();

        var bookings = await _lessonProgressRepository.GetByInstructorIdAsync(instructorId, cancellationToken);

        return bookings
            .Select(BookingMapper.MapToDto)
            .ToList();
    }
    
    public async Task<List<BookingDto>> GetBookingsByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student == null)
            throw new StudentNotFoundException(studentId);

        var bookings = await _lessonProgressRepository.GetByStudentIdAsync(studentId, cancellationToken);

        return bookings
            .Select(BookingMapper.MapToDto)
            .ToList();
    }
}