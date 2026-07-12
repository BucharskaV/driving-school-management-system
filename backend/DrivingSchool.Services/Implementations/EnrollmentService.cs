using DrivingSchool.Domain.Exceptions;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Domain.Models;
using DrivingSchool.Services.Interfaces;

namespace DrivingSchool.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(ICourseRepository courseRepository, IStudentRepository studentRepository, IEnrollmentRepository enrollmentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task EnrollStudentInCourseAsync(int courseId, int studentId, CancellationToken cancellationToken = default)
    {
        var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
        if (course is null)
            throw new CourseNotFoundException(courseId);
        
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student is null)
            throw new StudentNotFoundException(studentId);

        var enrollment = await _enrollmentRepository.GetByIdAsync(studentId, courseId, cancellationToken);
        if (enrollment is not null)
            throw new DuplicateEnrollmentException();

        var enrollmentToAdd = new Enrollment(student, course);
        await _enrollmentRepository.AddAsync(enrollmentToAdd, cancellationToken);
    }
}