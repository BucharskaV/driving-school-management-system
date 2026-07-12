using DrivingSchool.Domain.Models;
using DrivingSchool.Services.DTOs;

namespace DrivingSchool.Services.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllCoursesByCategoryIdAsync(int categoryId, CancellationToken cancellationToken);
}