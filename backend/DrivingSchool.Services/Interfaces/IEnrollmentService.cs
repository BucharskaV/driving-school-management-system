using DrivingSchool.Domain.Models;

namespace DrivingSchool.Services.Interfaces;

public interface IEnrollmentService
{
    Task EnrollStudentInCourseAsync(int courseId, int studentId, CancellationToken cancellationToken = default);
}