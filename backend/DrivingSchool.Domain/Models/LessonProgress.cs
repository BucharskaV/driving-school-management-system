using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class LessonProgress
{
    public ProgressStatus ProgressStatus{ get; set; }
    public DateTime? StartTime{ get; set; }
    private string? _note;
    public string? Note
    {
        get => _note;
        set
        {
            if (value != null && value.Length > 200)
            {
                throw new ArgumentException("Note cannot be longer than 200 characters");
            }
            _note = value?.Trim();
        }
    }
    
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