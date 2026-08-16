namespace DrivingSchool.Services.Contracts.Requests.Category;

public record CreateCategoryRequest(
    string Name,
    int MinimumAge
);