using DrivingSchool.Services.Contracts.Requests.Instructor;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class UpdateInstructorRequestValidator : AbstractValidator<UpdateInstructorRequest>
{
    public UpdateInstructorRequestValidator()
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

        RuleFor(x => x.BaseSalary)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base salary cannot be less than 0.");

        RuleFor(x => x.Bonus)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Bonus.HasValue)
            .WithMessage("Bonus cannot be less than 0.");

        RuleFor(x => x.DrivingLicenseNumber)
            .Matches(@"^[A-Z]{1,2}[0-9A-Z]{6,14}$")
            .When(x =>
                !string.IsNullOrWhiteSpace(x.DrivingLicenseNumber))
            .WithMessage("Invalid driving license number.");

        RuleFor(x => x.MedicalCertificateNumber)
            .Matches(@"^[0-9]{6,12}$")
            .When(x =>
                !string.IsNullOrWhiteSpace(x.MedicalCertificateNumber))
            .WithMessage("Invalid medical certificate number.");
    }
}