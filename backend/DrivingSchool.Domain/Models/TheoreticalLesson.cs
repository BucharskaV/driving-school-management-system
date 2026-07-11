namespace DrivingSchool.Domain.Models;

public class TheoreticalLesson : Lesson
{
    public string Topic { get; set; }
    public string? RoomNumber { get; set; }
    public bool IsOnline { get; set; }
    
    private TheoreticalLesson() : base(){}

    public TheoreticalLesson(Course course, string name, int sequenceNumber, TimeSpan duration, string topic, bool isOnline, string? roomNumber = null) : base(course, name, sequenceNumber, duration)
    {
        Topic = topic;
        IsOnline = isOnline;
        if (!IsOnline)
        {
            if (String.IsNullOrEmpty(roomNumber))
                throw new ArgumentNullException("Room number cannot be empty");
            RoomNumber = roomNumber;
        }
    }
}