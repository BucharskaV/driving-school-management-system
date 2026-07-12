namespace DrivingSchool.Domain.Models;

public class Category
{
    public int Id { get; private init; }
    public string Name{ get; set; }
    public int MinimumAge{ get; set; }
    public virtual ICollection<Course> Courses { get; set; } = [];

    private Category (){}
    public Category(string name, int minimumAge)
    {
        Name = name;
        MinimumAge = minimumAge;
    }
}