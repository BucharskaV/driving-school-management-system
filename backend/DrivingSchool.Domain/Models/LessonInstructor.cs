using System.Text.RegularExpressions;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class LessonInstructor
{
    public int LessonId { get; private set; }
    public virtual Lesson Lesson { get; private set; }
    public int InstructorId { get; private set; }
    public virtual Instructor Instructor { get; private set; }
    
    private string _instructorCode;
    public string InstructorCode
    {
        get => _instructorCode;
        init
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Instructor's code cannot be empty");
            }
            Regex codeRegex = new Regex("^[A-Z]{2}\\d{5}$");
            if (!codeRegex.IsMatch(value.Trim()))
            {
                throw new ArgumentException("Invalid instructor's code. The format is: 2 uppercase letters followed by 5 digits");
            }
            _instructorCode = value.Trim();
        }
    }
    
    private LessonInstructor() {}

    public LessonInstructor(Lesson lesson, Instructor instructor)
    {
        var requiredType = lesson switch
        {
            PracticalLesson => InstructorType.PracticalInstructor,
            TheoreticalLesson => InstructorType.TheoreticalInstructor,
            _ => throw new InvalidOperationException()
        };

        if (!instructor.CanTeach(requiredType))
            throw new InvalidOperationException("Instructor cannot be assigned to this lesson");
        
        Lesson = lesson;
        LessonId = lesson.Id;
        Instructor = instructor;
        InstructorId = instructor.Id;
        InstructorCode = instructor.InstructorCode;
    }
}