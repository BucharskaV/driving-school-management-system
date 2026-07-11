namespace DrivingSchool.Domain.Models;

public class Certification
{
    public int Id { get; private init; }
    public string Description { get; set; }
    public int? InstructorId { get; set; }
    public virtual Instructor? Instructor { get; set; }

    private Certification(){}
    public Certification(string description)
    {
        Description = description;
    }
}