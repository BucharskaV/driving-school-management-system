using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CourseService(ICourseRepository courseRepository, ICategoryRepository categoryRepository)
    {
        _courseRepository = courseRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IEnumerable<CourseDto>> GetAllCoursesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
            throw new CategoryNotFoundException(categoryId);

        var courses = await _courseRepository.GetAllCoursesByCategoryId(categoryId, cancellationToken);

        return courses.Select(course => new CourseDto(
            course.Id,
            course.Title,
            course.Price
        ));
    }
}