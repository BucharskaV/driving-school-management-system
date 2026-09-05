using DrivingSchool.Services.Contracts.Requests.Category;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Category name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Category name cannot be longer than 50 characters.");

        RuleFor(x => x.MinimumAge)
            .GreaterThanOrEqualTo(14)
            .WithMessage("Minimum age for driving category cannot be less than 14.");
    }
}