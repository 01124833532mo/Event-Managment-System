using EventManagment.Shared.Models.Sponsers;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.Sponsers
{
    public class CreateSponserValidation : AbstractValidator<CreateSponserDto>
    {
        public CreateSponserValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.LogoUrl)
                .NotNull()
                .WithMessage("LogoUrl is required.");



            RuleFor(x => x.Website)
                .NotEmpty()
                .WithMessage("Website is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Website must be a valid URL.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

        }
    }
}
