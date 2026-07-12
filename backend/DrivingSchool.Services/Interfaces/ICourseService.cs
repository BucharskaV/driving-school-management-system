using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetCoursesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
    Task<IEnumerable<EnrolledCourseDto>> GetEnrolledCoursesByStudentIdAsync(int studentId, CancellationToken cancellationToken);
}