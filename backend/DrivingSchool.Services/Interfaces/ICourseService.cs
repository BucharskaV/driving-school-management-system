using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Contracts.Requests.Course;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetCoursesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesByStudentIdAsync(int studentId, CancellationToken cancellationToken);
    Task<List<CourseWithCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CourseWithCategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CourseWithCategoryDto> CreateAsync(CreateCourseRequest request, CancellationToken cancellationToken = default);
    Task<CourseWithCategoryDto> UpdateAsync(int id, UpdateCourseRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}