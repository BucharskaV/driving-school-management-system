namespace DrivingSchool.Domain.Models;

public abstract class Lesson
{
    public int Id { get; private init; }
    public string Name { get; set; }
    public int SequenceNumber { get; set; }
    public TimeSpan Duration { get; set; }
    
    public virtual ICollection<LessonProgress> LessonProgresses => [];
    public int CourseId { get; private init; }
    public virtual Course Course { get; private init; }
    public virtual ICollection<LessonInstructor> LessonInstructors => [];
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