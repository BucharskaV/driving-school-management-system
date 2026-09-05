using DrivingSchool.Services.Contracts.Requests;
using FluentValidation;

namespace DrivingSchool.Services.Validators;

public class BookLessonRequestValidator : AbstractValidator<BookLessonRequest>
{
    public BookLessonRequestValidator()
    {
        RuleFor(x => x.LessonId)
            .GreaterThan(0)
            .WithMessage("Lesson ID must be greater than 0.");

        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0.");

        RuleFor(x => x.InstructorId)
            .GreaterThan(0)
            .WithMessage("Instructor ID must be greater than 0.");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required.");

        RuleFor(x => x)
            .Must(x => x.EndTime > x.StartTime)
            .WithMessage("End time must be later than start time.");
    }
}