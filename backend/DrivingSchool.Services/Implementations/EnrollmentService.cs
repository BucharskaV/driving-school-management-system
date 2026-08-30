using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Implementations;

public class EnrollmentService(
    ICourseRepository courseRepository,
    IStudentRepository studentRepository,
    IEnrollmentRepository enrollmentRepository)
    : IEnrollmentService
{
    public async Task EnrollStudentInCourseAsync(int courseId, int studentId, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(courseId, cancellationToken);
        if (course is null)
            throw new CourseNotFoundException(courseId);
        
        var student = await studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student is null)
            throw new StudentNotFoundException(studentId);

        var enrollment = await enrollmentRepository.GetByIdAsync(studentId, courseId, cancellationToken);
        if (enrollment is not null)
            throw new DuplicateEnrollmentException();

        var enrollmentToAdd = new Enrollment(student, course);
        await enrollmentRepository.AddAsync(enrollmentToAdd, cancellationToken);
    }
}