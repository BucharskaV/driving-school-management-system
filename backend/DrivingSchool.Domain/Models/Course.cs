namespace DrivingSchool.Domain.Models;

public class Course
{
    public int Id { get; private init; }
    public string Title { get; set; }
    public decimal Price { get; set; }
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