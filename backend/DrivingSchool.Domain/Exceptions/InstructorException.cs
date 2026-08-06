namespace DrivingSchool.Domain.Exceptions;

public class InstructorException : Exception
{
    protected InstructorException(string message) : base(message) { }
}
public class InstructorNotFoundException : InstructorException
{
    public InstructorNotFoundException() : base($"Instructor not found.") { }
}
public class SpecializationNotFoundException : InstructorException
{
    public SpecializationNotFoundException() : base($"Specialization not found.") { }
}