using DrivingSchool.Services.Contracts.Requests.Lesson;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class CreatePracticalLessonRequestValidator : AbstractValidator<CreatePracticalLessonRequest>
{
    public CreatePracticalLessonRequestValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Lesson name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Lesson name cannot be longer than 50 characters.");

        RuleFor(x => x.SequenceNumber)
            .GreaterThan(0)
            .WithMessage("Sequence number must be greater than 0.");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero)
            .WithMessage("Lesson duration must be greater than 0.");

        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("Car ID must be greater than 0.");

        RuleFor(x => x.StartLocationId)
            .GreaterThan(0)
            .WithMessage("Start location ID must be greater than 0.");
    }
}

public class UpdatePracticalLessonRequestValidator : AbstractValidator<UpdatePracticalLessonRequest>
{
    public UpdatePracticalLessonRequestValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Lesson name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Lesson name cannot be longer than 50 characters.");

        RuleFor(x => x.SequenceNumber)
            .GreaterThan(0)
            .WithMessage("Sequence number must be greater than 0.");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero)
            .WithMessage("Lesson duration must be greater than 0.");

        RuleFor(x => x.CarId)
            .GreaterThan(0)
            .WithMessage("Car ID must be greater than 0.");

        RuleFor(x => x.StartLocationId)
            .GreaterThan(0)
            .WithMessage("Start location ID must be greater than 0.");
    }
}

public class CreateTheoreticalLessonRequestValidator : AbstractValidator<CreateTheoreticalLessonRequest>
{
    public CreateTheoreticalLessonRequestValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Lesson name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Lesson name cannot be longer than 50 characters.");

        RuleFor(x => x.SequenceNumber)
            .GreaterThan(0)
            .WithMessage("Sequence number must be greater than 0.");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero)
            .WithMessage("Lesson duration must be greater than 0.");

        RuleFor(x => x.Topic)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Topic cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Topic cannot be longer than 50 characters.");

        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .When(x => !x.IsOnline)
            .WithMessage("Room number is required for an offline lesson.")
            .MaximumLength(10)
            .When(x => !string.IsNullOrWhiteSpace(x.RoomNumber))
            .WithMessage("Room number cannot be longer than 10 characters.");

        RuleFor(x => x.RoomNumber)
            .Empty()
            .When(x => x.IsOnline)
            .WithMessage("Room number cannot be specified for an online lesson.");
    }
}

public class UpdateTheoreticalLessonRequestValidator : AbstractValidator<UpdateTheoreticalLessonRequest>
{
    public UpdateTheoreticalLessonRequestValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Lesson name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Lesson name cannot be longer than 50 characters.");

        RuleFor(x => x.SequenceNumber)
            .GreaterThan(0)
            .WithMessage("Sequence number must be greater than 0.");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero)
            .WithMessage("Lesson duration must be greater than 0.");

        RuleFor(x => x.Topic)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("Topic cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Topic cannot be longer than 50 characters.");

        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .When(x => !x.IsOnline)
            .WithMessage("Room number is required for an offline lesson.")
            .MaximumLength(10)
            .When(x => !string.IsNullOrWhiteSpace(x.RoomNumber))
            .WithMessage("Room number cannot be longer than 10 characters.");

        RuleFor(x => x.RoomNumber)
            .Empty()
            .When(x => x.IsOnline)
            .WithMessage("Room number cannot be specified for an online lesson.");
    }
}