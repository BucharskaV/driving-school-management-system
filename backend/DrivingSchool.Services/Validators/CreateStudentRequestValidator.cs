using DrivingSchool.Services.Contracts.Requests.Student;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("First name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("First name cannot be longer than 50 characters.");

        RuleFor(x => x.LastName)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Last name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Last name cannot be longer than 50 characters.");

        RuleFor(x => x.Pesel)
            .NotEmpty()
            .WithMessage("PESEL number cannot be empty.")
            .Matches(@"^\d{11}$")
            .WithMessage("PESEL must consist of exactly 11 digits.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty.")
            .Matches(@"^(\+48)?\d{9}$")
            .WithMessage("Invalid phone number.");

        RuleFor(x => x.Email)
            .MaximumLength(50)
            .WithMessage("Email cannot be longer than 50 characters.")
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email address.");

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date of birth cannot be in the future.");
    }
}