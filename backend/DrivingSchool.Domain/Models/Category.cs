namespace DrivingSchool.Domain.Models;

public class Category
{
    public int Id { get; private init; }
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Name cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Name cannot be longer than 50 characters");
            }
            _name = value.Trim();
        }
    }
    
    private int _minimumAge;
    public int MinimumAge
    {
        get => _minimumAge;
        set
        {
            if (value < 14)
            {
                throw new ArgumentException("Minimum age for driving category cannot be less than 14");
            }
            _minimumAge = value;
        }
    }
    public virtual ICollection<Course> Courses { get; set; } = [];

    private Category (){}
    public Category(string name, int minimumAge)
    {
        Name = name;
        MinimumAge = minimumAge;
    }
}