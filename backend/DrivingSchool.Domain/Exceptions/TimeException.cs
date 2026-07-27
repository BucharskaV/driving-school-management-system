namespace DrivingSchool.Domain.Exceptions;

public class TimeException : Exception
{
    protected TimeException(string message) : base(message) { }
}
public class TimeIsRequiredException : TimeException
{
    public TimeIsRequiredException() : base($"Time is required to proceed.") { }
}
public class StartTimeIsInPastException : TimeException
{
    public StartTimeIsInPastException() : base($"Booking start time cannot be in the past") { }
}
public class OfficeHoursException : TimeException
{
    public OfficeHoursException(TimeSpan openingTime, TimeSpan closingTime)
        : base($"Lesson must be within office hours ({openingTime:hh\\:mm}–{closingTime:hh\\:mm}).")
    {
    }
}