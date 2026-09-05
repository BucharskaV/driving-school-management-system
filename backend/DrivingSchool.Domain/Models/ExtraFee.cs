namespace DrivingSchool.Domain.Models;

public class ExtraFee
{
    public int Id { get; private init; }
    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        init
        {
            if (value <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero");
            }
            _amount = value;
        }
    }
    
    private DateTime _dateOfPayment;
    public DateTime DateOfPayment
    {
        get => _dateOfPayment;
        init
        {
            if (value > DateTime.UtcNow)
            {
                throw new ArgumentException("Date of payment cannot be in the future");
            }
            _dateOfPayment = value;
        }
    }
    public int StudentId { get; private init; }
    public int LessonId { get; private init; }
    public virtual LessonProgress LessonProgress { get; private  init; }
    
    private ExtraFee(){}
    public ExtraFee(LessonProgress lp, decimal amount)
    {
        Amount = amount;
        DateOfPayment = DateTime.UtcNow;
        LessonProgress = lp;
        StudentId = lp.StudentId;
        LessonId = lp.LessonId;
    }
}