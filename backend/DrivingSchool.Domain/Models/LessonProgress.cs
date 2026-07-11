using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class LessonProgress
{
    private ProgressStatus _progressStatus;
    public ProgressStatus ProgressStatus
    {
        get => _progressStatus; 
        set => _progressStatus = value;
    }
    
    private DateTime? _startTime;
    public DateTime? StartTime
    {
        get => _startTime;
        set
        {
            if (value == null)
            {
                _startTime = null;
                EndTime = null;
                return;
            }
            EnsureBooked();
            
            var startLocal = value.Value.ToLocalTime();
            if (startLocal < DateTime.Now)
                throw new ArgumentException("Booking start time cannot be in the past");
            var endLocal = Lesson != null
                ? startLocal + Lesson.Duration
                : startLocal;

            var openingTime = TimeSpan.FromHours(8);
            var closingTime = TimeSpan.FromHours(20);
            if (startLocal.TimeOfDay < openingTime || endLocal.TimeOfDay > closingTime)
                throw new ArgumentException("Lesson must be scheduled between 08:00 and 20:00");

            _startTime = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);

            if (Lesson != null)
                EndTime = _startTime.Value + Lesson.Duration;
        }
    }
    
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

    public DateTime? EndTime
    {
        get => _endTime;
        private set => _endTime = value;
    }
    
    public int StudentId { get; private set; }
    public virtual Student Student { get; private set; }

    public int LessonId { get; private set; }
    public virtual Lesson Lesson { get; private set; }
    
    public int? InstructorId { get; set; }
    private Instructor? _instructor;
    public virtual Instructor? Instructor
    {
        get => _instructor;
        set
        {
            if (value != null) 
                EnsureBooked();
            _instructor = value;
        }
    }
    
    public virtual ExtraFee? ExtraFee { get; private set; }

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
    
    private void EnsureBooked()
    {
        if (ProgressStatus != ProgressStatus.Booked)
            throw new InvalidOperationException("Only booked lessons can be modified.");
    }

    public ExtraFee CreateExtraFee(decimal amount)
    {
        ExtraFee fee = new ExtraFee(this, amount, DateTime.UtcNow);
        ExtraFee = fee;
        return fee;
    }
}