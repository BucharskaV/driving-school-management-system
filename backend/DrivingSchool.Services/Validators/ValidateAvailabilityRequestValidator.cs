using DrivingSchool.Services.Contracts.Requests;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class ValidateAvailabilityRequestValidator : AbstractValidator<ValidateAvailabilityRequest>
{
    public ValidateAvailabilityRequestValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0.");

        RuleFor(x => x.LessonId)
            .GreaterThan(0)
            .WithMessage("Lesson ID must be greater than 0.");

        RuleFor(x => x.Start)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(x => x.End)
            .NotEmpty()
            .WithMessage("End time is required.");

        RuleFor(x => x)
            .Must(x => x.End > x.Start)
            .WithMessage("End time must be later than start time.");
    }
}