namespace DrivingSchool.Domain.Exceptions;

public class CategoryException: Exception
{
    protected CategoryException(string message) : base(message) { }
}

public class CategoryNotFoundException : CategoryException
{
    public CategoryNotFoundException(int id) : base($"Category with ID {id} was not found.") { }
}