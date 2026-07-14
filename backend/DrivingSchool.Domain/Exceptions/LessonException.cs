namespace DrivingSchool.Domain.Exceptions;

public class LessonException : Exception
{
    protected LessonException(string message) : base(message) { }
}
public class LessonNotFoundException : LessonException
{
    public LessonNotFoundException(int id) : base($"Lesson with ID {id} was not found.") { }
}