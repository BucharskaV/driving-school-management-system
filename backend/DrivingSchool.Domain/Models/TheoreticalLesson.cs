namespace DrivingSchool.Domain.Models;

public class TheoreticalLesson : Lesson
{
    private string _topic;
    public string Topic
    {
        get => _topic;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Topic name cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Topic cannot be longer than 50 characters");
            }
            _topic = value.Trim();
        }
    }
    
    private string? _roomNumber;
    public string? RoomNumber
    {
        get => _roomNumber;
        set
        {
            if (IsOnline)
                throw new InvalidOperationException("The room number cannot be set for online class");
            if (value != null && value.Length > 10)
            {
                throw new ArgumentException("Room number cannot be longer than 10 characters");
            }
            _roomNumber = value.Trim();
        }
    }
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