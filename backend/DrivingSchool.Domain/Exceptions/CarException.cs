namespace DrivingSchool.Domain.Exceptions;

public class CarException: Exception
{
    protected CarException(string message) : base(message) { }
}

public class CarNotFoundException : CarException
{
    public CarNotFoundException(int id) : base($"Car with ID {id} was not found.") { }
}