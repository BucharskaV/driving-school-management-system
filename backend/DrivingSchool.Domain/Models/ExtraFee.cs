namespace DrivingSchool.Domain.Models;

public class ExtraFee
{
    public int Id { get; private init; }
    public decimal Amount { get; init; }
    public DateTime DateOfPayment { get; init; }
    public int StudentId { get; private init; }
    public int LessonId { get; private init; }
    public virtual LessonProgress LessonProgress { get; private  init; }
    
    private ExtraFee(){}
    internal ExtraFee(LessonProgress lp, decimal amount, DateTime dateOfPayment)
    {
        Amount = amount;
        DateOfPayment = dateOfPayment;
        LessonProgress = lp;
        StudentId = lp.StudentId;
        LessonId = lp.LessonId;
    }
}