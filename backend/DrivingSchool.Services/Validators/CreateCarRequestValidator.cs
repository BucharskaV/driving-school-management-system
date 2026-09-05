using DrivingSchool.Services.Contracts.Requests.Car;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class CreateCarRequestValidator : AbstractValidator<CreateCarRequest>
{
    public CreateCarRequestValidator()
    {
        RuleFor(x => x.Brand)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Brand cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Brand cannot be longer than 50 characters.");

        RuleFor(x => x.Model)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Model cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Model cannot be longer than 50 characters.");

        RuleFor(x => x.RegistrationNumber)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Car registration number cannot be empty.")
            .Matches(@"^[A-Z]{1,3}\s?[A-Z0-9]{4,6}$")
            .WithMessage("Invalid car registration number.");
    }
}