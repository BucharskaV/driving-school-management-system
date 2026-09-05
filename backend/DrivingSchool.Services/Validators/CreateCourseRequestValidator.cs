using DrivingSchool.Services.Contracts.Requests.Course;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.Title)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Course title cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Course title cannot be longer than 50 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Course price cannot be less than 0.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Category ID must be greater than 0.");
    }
}