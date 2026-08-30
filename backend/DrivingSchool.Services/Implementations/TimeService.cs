using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Options;
using Microsoft.Extensions.Options;

namespace DrivingSchool.Services.Implementations;

public class TimeService : ITimeService
{
    private readonly OfficeHoursOptions _options;
    private readonly TimeZoneInfo _timeZone;

    public TimeService(IOptions<OfficeHoursOptions> options)
    {
        _options = options.Value;
        _timeZone = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZoneId);
    }

    public bool IsWithinOfficeHours(DateTime start, DateTime end)
    {
        var localStart = ToLocalTime(start);
        var localEnd = ToLocalTime(end);

        return localStart.TimeOfDay >= _options.OpeningTime
               && localEnd.TimeOfDay <= _options.ClosingTime;
    }

    public TimeSpan CalculateDuration(DateTime start, DateTime end) => end - start;

    public DateTime ToLocalTime(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc),
            _timeZone);
    }

    public DateTime ToUtc(DateTime localDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified),
            _timeZone);
    }
}