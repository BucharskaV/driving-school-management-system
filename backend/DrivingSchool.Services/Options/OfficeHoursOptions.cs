namespace DrivingSchool.Services.Options;

public class OfficeHoursOptions
{
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; } 
    public string TimeZoneId { get; set; } = "Europe/Warsaw";
}