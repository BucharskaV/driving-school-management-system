namespace DrivingSchool.Domain.Exceptions;

public class StudentException: Exception
{
    protected StudentException(string message) : base(message) { }
}

public class StudentNotFoundException : StudentException
{
    public StudentNotFoundException(int id) : base($"Student with ID {id} was not found.") { }
}
public class StudentUnavailableException : StudentException
{
    public StudentUnavailableException() : base($"Student is unavailable during selected timeslot.") { }
}