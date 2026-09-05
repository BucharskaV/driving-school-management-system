using DrivingSchool.Domain.Enums;
using DrivingSchool.Services.Contracts.Requests.Instructor;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class CreateInstructorRequestValidator : AbstractValidator<CreateInstructorRequest>
{
    public CreateInstructorRequestValidator()
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

        RuleFor(x => x.InstructorCode)
            .NotEmpty()
            .WithMessage("Instructor code cannot be empty.")
            .Matches(@"^[A-Z]{2}\d{5}$")
            .WithMessage(
                "Invalid instructor code. The format is 2 uppercase letters followed by 5 digits.");

        RuleFor(x => x.BaseSalary)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base salary cannot be less than 0.");

        RuleFor(x => x.Bonus)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Bonus.HasValue)
            .WithMessage("Bonus cannot be less than 0.");

        RuleFor(x => x.Specializations)
            .NotEmpty()
            .WithMessage("Instructor must have at least one specialization.");

        RuleFor(x => x.DrivingLicenseNumber)
            .Matches(@"^[A-Z]{1,2}[0-9A-Z]{6,14}$")
            .When(x =>
                x.DrivingLicenseNumber != null &&
                x.DrivingLicenseNumber.Trim() != "")
            .WithMessage("Invalid driving license number.");

        RuleFor(x => x.MedicalCertificateNumber)
            .Matches(@"^[0-9]{6,12}$")
            .When(x =>
                x.MedicalCertificateNumber != null &&
                x.MedicalCertificateNumber.Trim() != "")
            .WithMessage("Invalid medical certificate number.");

        RuleFor(x => x.DrivingLicenseNumber)
            .NotEmpty()
            .When(x => x.Specializations.Contains(
                InstructorType.PracticalInstructor))
            .WithMessage(
                "Driving license is required for a practical instructor.");

        RuleFor(x => x.MedicalCertificateNumber)
            .NotEmpty()
            .When(x => x.Specializations.Contains(
                InstructorType.PracticalInstructor))
            .WithMessage(
                "Medical certificate is required for a practical instructor.");
    }
}