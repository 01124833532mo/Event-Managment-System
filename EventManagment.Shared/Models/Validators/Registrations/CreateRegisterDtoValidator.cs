using EventManagment.Shared.Models.Registrations;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.Registrations
{
    public class CreateRegisterDtoValidator : AbstractValidator<CreateRegisterDto>
    {
        public CreateRegisterDtoValidator()
        {
            RuleFor(x => x.Eventid).NotEmpty().WithMessage("Event Id is required")
                    .NotNull();
            RuleFor(x => x.ServicePrice).NotEmpty().WithMessage("Event Price is required");
        }
    }
}
