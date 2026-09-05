namespace DrivingSchool.Domain.Models;

public abstract class Lesson
{
    public int Id { get; private init; }
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Name cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Name cannot be longer than 50 characters");
            }
            _name = value.Trim();
        }
    }
    public int SequenceNumber { get; set; }
    public TimeSpan Duration { get; set; }
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = [];
    public int CourseId { get; private init; }
    public virtual Course Course { get; private init; }
    public virtual ICollection<LessonInstructor> LessonInstructors { get; set; } = [];
    protected Lesson(){}
    protected Lesson(Course course, string name, int sequenceNumber, TimeSpan duration)
    {
        Name = name;
        SequenceNumber = sequenceNumber;
        Duration = duration;
        Course = course;
        CourseId = course.Id;
    }
} 