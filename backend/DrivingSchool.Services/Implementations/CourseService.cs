using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Course;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Mappers;

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

        return courses.Select(CourseMapper.MapToDto);
    }

    public async Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesByStudentIdAsync(int studentId, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student is null)
            throw new StudentNotFoundException(studentId);

        var courses = await _courseRepository.GetEnrolledCoursesByStudentIdAsync(studentId, cancellationToken);

        return courses
            .Select(CourseMapper.MapToEnrolledDto)
            .ToList();
    }

    public async Task<List<CourseWithCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var courses = await _courseRepository.GetAllAsync(cancellationToken);

        return courses
            .Select(CourseMapper.MapToDtoWithCategory)
            .ToList();
    }

    public async Task<CourseWithCategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);

        if (course == null)
            throw new CourseNotFoundException(id);

        return CourseMapper.MapToDtoWithCategory(course);
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

        return CourseMapper.MapToDtoWithCategory(course);
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
        await _courseRepository.UpdateAsync(course, cancellationToken);

        return CourseMapper.MapToDtoWithCategory(course);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        if (course == null)
            throw new CourseNotFoundException(id);

        await _courseRepository.DeleteAsync(course, cancellationToken);
    }
}