namespace DrivingSchool.Domain.Exceptions;

public class EnrollmentException: Exception
{
    protected EnrollmentException(string message) : base(message) { }
}

public class DuplicateEnrollmentException : EnrollmentException
{
    public DuplicateEnrollmentException() : base($"Student already enrolled in this course.") { }
}