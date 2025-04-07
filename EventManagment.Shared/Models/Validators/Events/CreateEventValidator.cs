using EventManagment.Shared.Models.Events;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.Events
{
    public class CreateEventValidator : AbstractValidator<EventDto>
    {
        public CreateEventValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(20).WithMessage("Tiltle Must be Less Than 20 Character");


            RuleFor(x => x.Description)
              .NotEmpty().WithMessage("Description is required.")
              .MaximumLength(120).WithMessage("Description Must be Less Than 20 Character");


            RuleFor(x => x.Location)
             .NotEmpty().WithMessage("Location is required.")
             .MaximumLength(100).WithMessage("Description Must be Less Than 100 Character");

            RuleFor(x => x.Data).NotEmpty();
            RuleFor(x => x.MaxAttendees).NotEmpty()

                                         .NotNull().WithMessage("Must Set Numbers For Attendee")
                                         ;

            RuleFor(x => x.SponserId)
                .NotEmpty().WithMessage("Sponser is required.")
                .NotNull().WithMessage("Sponser is required.")
                .GreaterThan(0).WithMessage("Sponser Must Be Greater Than 0");




        }

    }
}
