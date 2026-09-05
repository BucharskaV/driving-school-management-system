using DrivingSchool.Services.Contracts.Requests.Car;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class UpdateCarRequestValidator : AbstractValidator<UpdateCarRequest>
{
    public UpdateCarRequestValidator()
    {
        RuleFor(x => x.RegistrationNumber)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Car registration number cannot be empty.")
            .Matches(@"^[A-Z]{1,3}\s?[A-Z0-9]{4,6}$")
            .WithMessage("Invalid car registration number.");
    }
}