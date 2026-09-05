namespace DrivingSchool.Domain.Models;

public class Enrollment
{
    private DateTime _enrollmentDate;
    public DateTime EnrollmentDate
    {
        get => _enrollmentDate;
        init
        {
            if (value > DateTime.UtcNow)
            {
                throw new ArgumentException("Enrollment date cannot be in the future");
            }
            _enrollmentDate = value;
        }
    }
    public bool IsPassed { get; set; }
    public int StudentId { get; private init; }
    public virtual Student Student { get; private init; }
    public int CourseId { get; private init; }
    public virtual Course Course { get; private init; }
    
    private Enrollment(){}
    public Enrollment(Student student, Course course)
    {
        EnrollmentDate = DateTime.UtcNow;
        Student = student;
        StudentId = student.Id;
        Course = course;
        CourseId = course.Id;
        IsPassed = false;
    }
}