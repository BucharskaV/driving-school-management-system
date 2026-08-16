namespace DrivingSchool.Services.Contracts.Requests.Category;

public record UpdateCategoryRequest(
    string Name,
    int MinimumAge
);