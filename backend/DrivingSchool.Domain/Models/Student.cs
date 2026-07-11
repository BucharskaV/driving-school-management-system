namespace DrivingSchool.Domain.Models;

public class Student : User
{
    public DateTime DateOfBirth { get; set; }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
    
    public virtual ICollection<Enrollment> Enrollments => [];
    public virtual ICollection<LessonProgress> LessonProgresses => [];
    
    private Student() : base(){}

    public Student(string firstName, string lastName, string pesel, string phoneNumber, string email, DateTime dateOfBirth) : base(firstName, lastName, pesel, phoneNumber, email)
    {
        DateOfBirth = dateOfBirth;
    }

    public Student(string firstName, string lastName, string pesel, string phoneNumber, DateTime dateOfBirth) : base(firstName, lastName, pesel, phoneNumber)
    {
        DateOfBirth = dateOfBirth;
    }
}