namespace DrivingSchool.Services.Interfaces;

public interface ITimeService
{
    bool IsWithinOfficeHours(DateTime start, DateTime end);
    TimeSpan CalculateDuration(DateTime start, DateTime end);
    DateTime ToLocalTime(DateTime utcDateTime);
    DateTime ToUtc(DateTime localDateTime);
}