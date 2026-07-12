using System.Text.RegularExpressions;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class LessonInstructor
{
    public int LessonId { get; private init; }
    public virtual Lesson Lesson { get; private init; }
    public int InstructorId { get; private init; }
    public virtual Instructor Instructor { get; private init; }
    public string InstructorCode{ get; private init; }
    
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