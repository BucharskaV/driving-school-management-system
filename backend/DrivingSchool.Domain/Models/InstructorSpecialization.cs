using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Models;

public class InstructorSpecialization
{
    public InstructorType Type { get; init; }
    public int InstructorId { get; init; }
    public virtual Instructor Instructor { get; init; }
    
    private InstructorSpecialization() {}
    public InstructorSpecialization(Instructor instructor, InstructorType type)
    {
        Instructor = instructor;
        InstructorId = instructor.Id;
        Type = type;
    }
}