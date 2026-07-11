namespace DrivingSchool.Domain.Models;

public class PracticalLesson : Lesson
{
    public int StartLocationId { get; set; }
    public virtual Address StartLocation { get; set; }
    public int CarId { get; set; }
    public virtual Car Car { get; set; }
    
    private PracticalLesson() : base(){}
    public PracticalLesson(Course course, Car car, string name, int sequenceNumber, TimeSpan duration, Address a) : base(course, name, sequenceNumber, duration)
    {
        StartLocation = a;
        StartLocationId = a.Id;
        Car = car;
        CarId = car.Id;
    }
}