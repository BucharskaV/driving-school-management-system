using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class Student : User
{
    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.UtcNow)
            {
                throw new ArgumentException("Date of birth cannot be in the future");
            }
            _dateOfBirth = value;
        }
    }

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
    
    public virtual ICollection<Enrollment> Enrollments { get; set; } = [];
    public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = [];
    
    private Student() : base(){}

    public Student(string firstName, string lastName, string pesel, Role role, string phoneNumber, string? email, DateTime dateOfBirth) : base(firstName, lastName, role, pesel, phoneNumber, email)
    {
        DateOfBirth = dateOfBirth;
    }

    public Student(string firstName, string lastName, string pesel, Role role, string phoneNumber, DateTime dateOfBirth) : base(firstName, lastName, role, pesel, phoneNumber)
    {
        DateOfBirth = dateOfBirth;
    }
}