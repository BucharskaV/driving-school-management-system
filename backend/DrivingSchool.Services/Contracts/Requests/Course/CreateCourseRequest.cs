namespace DrivingSchool.Services.Contracts.Requests.Course;

public record CreateCourseRequest(
    string Title,
    decimal Price,
    int CategoryId
);