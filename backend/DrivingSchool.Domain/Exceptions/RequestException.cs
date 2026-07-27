namespace DrivingSchool.Domain.Exceptions;

public class RequestException : Exception
{
    protected RequestException(string message) : base(message) { }
}
public class InvalidRequestException : RequestException
{
    public InvalidRequestException() : base("Invalid request format") { }
}