namespace DrivingSchool.Domain.Models;

public class Certification
{
    public int Id { get; private init; }
    private string _description;
    public string Description
    {
        get => _description;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Description cannot be empty");
            }
            if (value.Length > 200)
            {
                throw new ArgumentException("Description cannot be longer than 200 characters");
            }
            _description = value.Trim();
        }
    }
    public int? InstructorId { get; set; }
    public virtual Instructor? Instructor { get; set; }

    private Certification(){}
    public Certification(string description)
    {
        Description = description;
    }
}