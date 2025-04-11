using EventManagment.Shared.Models.Sesstions;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.Sesstions
{
    public class CreateSesstionDtoValidator : AbstractValidator<SesstionDto>
    {
        public CreateSesstionDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty()
                .WithMessage("Title Must Be Not Empity")
                .MaximumLength(100).WithMessage("Title Must Be Abouve 100 characters");

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage("Description Must Be Not Empity")
                .MaximumLength(500).WithMessage("Title Must Be Abouve 500 characters");

            RuleFor(x => x.EventId).NotEmpty().WithMessage("Event Id is required")
                    .NotNull();

            RuleFor(x => x.SpeakerId).NotEmpty().WithMessage("Speaker Id is required")
                  .NotNull();
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start Time is required")
                .NotNull();
            RuleFor(x => x.EndTime).NotEmpty().WithMessage("End Time is required")
                .NotNull();
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime)
                .WithMessage("Start Time must be less than End Time");








        }
    }
}
