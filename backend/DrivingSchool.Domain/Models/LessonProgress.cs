using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class LessonProgress
{
    public ProgressStatus ProgressStatus{ get; set; }
    public DateTime? StartTime{ get; set; }
    public string? Note { get; set; }
    
    private DateTime? _endTime;
    public DateTime? EndTime{ get; set; }
    
    public int StudentId { get; private init; }
    public virtual Student Student { get; private init; }

    public int LessonId { get; private init; }
    public virtual Lesson Lesson { get; private init; }
    
    public int? InstructorId { get; set; }
    private Instructor? _instructor;
    public virtual Instructor? Instructor{ get; set; }
    public int? ExtraFeeId { get; set; }
    public virtual ExtraFee? ExtraFee { get; init; }

    private LessonProgress(){}
    public LessonProgress(Student student, Lesson lesson, ProgressStatus progressStatus, string? note = null)
    {
        ProgressStatus = progressStatus;
        Note = note;
        Student = student;
        StudentId = student.Id;
        Lesson = lesson;
        LessonId = lesson.Id;
    }
}