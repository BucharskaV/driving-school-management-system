namespace DrivingSchool.Services.Contracts.Requests.Course;

public record UpdateCourseRequest(
    string Title,
    decimal Price,
    int CategoryId
);