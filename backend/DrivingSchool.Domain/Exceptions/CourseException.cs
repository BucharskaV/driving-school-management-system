namespace DrivingSchool.Domain.Exceptions;

public class CourseException: Exception
{
    protected CourseException(string message) : base(message) { }
}

public class CourseNotFoundException : CourseException
{
    public CourseNotFoundException(int id) : base($"Course with ID {id} was not found.") { }
}