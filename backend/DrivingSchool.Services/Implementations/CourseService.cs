using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Course;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStudentRepository _studentRepository;

    public CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository, IStudentRepository studentRepository)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
        _studentRepository = studentRepository;
    }
    
    public async Task<IEnumerable<CourseDto>> GetCoursesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
            throw new CategoryNotFoundException(categoryId);

        var courses = await _courseRepository.GetCoursesByCategoryIdAsync(categoryId, cancellationToken);

        return courses.Select(course => new CourseDto(
            course.Id,
            course.Title,
            course.Price,
            course.Lessons
                .OrderBy(l => l.SequenceNumber)
                .Select(MapLessonToDto)
                .ToList()
        ));
    }

    public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesByStudentIdAsync(int studentId, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student is null)
            throw new StudentNotFoundException(studentId);

        var courses = await _courseRepository.GetEnrolledCoursesByStudentIdAsync(studentId, cancellationToken);

        return courses.Select(course => new EnrolledCourseDto(
            course.Id,
            course.Title,
            course.Price,
            course.Lessons.Select(lesson =>
            {
                var progress = lesson.LessonProgresses.FirstOrDefault();
                var practical = lesson as PracticalLesson;
                var theory = lesson as TheoreticalLesson;

                return new LessonDto(
                    lesson.Id,
                    lesson.Name,
                    lesson.SequenceNumber,
                    lesson.Duration,

                    practical is null
                        ? null
                        : new CarDto(
                            practical.Car.Id,
                            practical.Car.Brand,
                            practical.Car.Model,
                            practical.Car.RegistrationNumber),

                    practical is null
                        ? null
                        : new AddressDto(
                            practical.StartLocation.Id,
                            practical.StartLocation.City,
                            practical.StartLocation.District,
                            practical.StartLocation.Street,
                            practical.StartLocation.HouseNumber),

                    theory?.Topic,
                    theory?.RoomNumber,
                    theory?.IsOnline,

                    progress is null
                        ? null
                        : new LessonProgressDto(
                            progress.StudentId,
                            progress.LessonId,
                            progress.ProgressStatus,
                            progress.StartTime,
                            progress.EndTime,
                            progress.Note,
                            progress.InstructorId,
                            progress.ExtraFee?.Id)
                );
            })
        ));
    }

    public async Task<List<CourseWithCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var courses = await _courseRepository.GetAllAsync(cancellationToken);

        return courses
            .Select(MapToDto)
            .ToList();
    }

    public async Task<CourseWithCategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);

        if (course == null)
            throw new CourseNotFoundException(id);

        return MapToDto(course);
    }

    public async Task<CourseWithCategoryDto> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            throw new CategoryNotFoundException(request.CategoryId);

        var course = new Course(
            category,
            request.Title,
            request.Price);

        await _courseRepository
            .AddAsync(course, cancellationToken);

        return MapToDto(course);
    }

    public async Task<CourseWithCategoryDto> UpdateAsync(int id, UpdateCourseRequest request, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        if (course == null)
            throw new CourseNotFoundException(id);

        var category = await _categoryRepository
            .GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            throw new CategoryNotFoundException(request.CategoryId);

        course.Title = request.Title;
        course.Price = request.Price;
        await _courseRepository
            .UpdateAsync(course, cancellationToken);

        return MapToDto(course);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        if (course == null)
            throw new CourseNotFoundException(id);

        await _courseRepository
            .DeleteAsync(course, cancellationToken);
    }

    private static CourseWithCategoryDto MapToDto(Course course)
    {
        return new CourseWithCategoryDto(
            course.Id,
            course.Title,
            course.Price,
            course.CategoryId,
            course.Category.Name);
    }
    private static LessonDto MapLessonToDto(Lesson lesson)
    {
        var practical = lesson as PracticalLesson;
        var theory = lesson as TheoreticalLesson;

        var progress = lesson.LessonProgresses
            .FirstOrDefault();

        return new LessonDto(
            lesson.Id,
            lesson.Name,
            lesson.SequenceNumber,
            lesson.Duration,

            practical?.Car is null
                ? null
                : new CarDto(
                    practical.Car.Id,
                    practical.Car.Brand,
                    practical.Car.Model,
                    practical.Car.RegistrationNumber),

            practical?.StartLocation is null
                ? null
                : new AddressDto(
                    practical.StartLocation.Id,
                    practical.StartLocation.City,
                    practical.StartLocation.District,
                    practical.StartLocation.Street,
                    practical.StartLocation.HouseNumber),

            theory?.Topic,
            theory?.RoomNumber,
            theory?.IsOnline,

            progress is null
                ? null
                : new LessonProgressDto(
                    progress.StudentId,
                    progress.LessonId,
                    progress.ProgressStatus,
                    progress.StartTime,
                    progress.EndTime,
                    progress.Note,
                    progress.InstructorId,
                    progress.ExtraFee?.Id)
        );
    }
}