using DrivingSchool.Services.Contracts.Requests.Auth;
using FluentValidation;
using Microsoft.Identity.Client;

namespace DrivingSchool.Services.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("First name cannot be longer than 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Last name cannot be longer than 50 characters.");

        RuleFor(x => x.Pesel)
            .NotEmpty()
            .WithMessage("PESEL number cannot be empty.")
            .Matches(@"^\d{11}$")
            .WithMessage("Invalid PESEL number. The PESEL must consist of exactly 11 digits.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty.")
            .Matches(@"^(\+48)?\d{9}$")
            .WithMessage("Invalid phone number. Use 9 digits, optionally with the +48 prefix.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Email cannot be longer than 50 characters.")
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100)
            .WithMessage("Password cannot be longer than 100 characters.");
    }
}