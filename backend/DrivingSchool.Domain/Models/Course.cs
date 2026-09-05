namespace DrivingSchool.Domain.Models;

public class Course
{
    public int Id { get; private init; }
    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            if(String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Title name cannot be empty");
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("Title cannot be longer than 50 characters");
            }
            _title = value.Trim();
        }
    }
    
    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Course price cannot be less than 0");
            }
            _price = value;
        }
    }

    public int CategoryId { get; private init; }
    public virtual Category Category { get; private init; }
    public virtual ICollection<Enrollment> Enrollments { get; set; } = [];
    public virtual ICollection<Lesson> Lessons { get; set; } = [];
    
    private Course(){}
    public Course(Category category, string title, decimal price)
    {
        ArgumentNullException.ThrowIfNull(category);
        Title = title;
        Price = price;
        Category = category;
        CategoryId = category.Id;
    }
}