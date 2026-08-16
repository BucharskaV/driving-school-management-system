namespace DrivingSchool.Services.DTOs;

public record CourseWithCategoryDto(
    int Id,
    string Title,
    decimal Price,
    int CategoryId,
    string CategoryName
);