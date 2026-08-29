namespace DrivingSchool.Domain.Exceptions;

public class LessonException : Exception
{
    protected LessonException(string message) : base(message) { }
}
public class LessonNotFoundException : LessonException
{
    public LessonNotFoundException(int id) : base($"Lesson with ID {id} was not found.") { }
}
public class LessonIsRequiredException : LessonException
{
    public LessonIsRequiredException() : base($"Lesson is required to proceed.") { }
}
public class ProgressNotFoundException : LessonException
{
    public ProgressNotFoundException() : base($"Progress was not found.") { }
}
public class CarUnavailableException : LessonException
{
    public CarUnavailableException() : base($"Car is unavailable during selected timeslot.") { }
}
public class RoomUnavailableException : LessonException
{
    public RoomUnavailableException() : base($"Car is unavailable during selected timeslot.") { }
}
public class InvalidNoteException : LessonException
{
    public InvalidNoteException() : base($"Note cannot be longer than 200 characters.") { }
}
public class AddressNotFoundException : LessonException
{
    public AddressNotFoundException(int id) : base($"Address was not found.") { }
}