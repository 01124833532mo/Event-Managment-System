using EventManagment.Shared.Models.Auth;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
         .NotEmpty().WithMessage("Email is required.")
         .EmailAddress().WithMessage("A valid email address is required.");

            // Rule for Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }

    }
}
