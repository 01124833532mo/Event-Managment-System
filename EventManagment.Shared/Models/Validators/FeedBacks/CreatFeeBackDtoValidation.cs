using EventManagment.Shared.Models.FeedBacks;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.FeedBacks
{
    public class CreatFeeBackDtoValidation : AbstractValidator<CreateFeedBackDto>
    {

        public CreatFeeBackDtoValidation()
        {
            RuleFor(x => x.Rate)
                .NotEmpty().WithMessage("Rate is required.")
                .InclusiveBetween(1, 5).WithMessage("Rate must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(500).WithMessage("Comment must be less than 500 characters.");

            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("Event ID is required.")
                .NotNull().WithMessage("Event ID cannot be null.");

        }

    }
}
