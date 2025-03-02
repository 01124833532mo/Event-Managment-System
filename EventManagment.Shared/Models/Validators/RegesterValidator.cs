using EventManagment.Shared.Models.Auth;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators
{
    public class RegesterValidator : AbstractValidator<RegisterDto>
    {
        public RegesterValidator()
        {
            RuleFor(e => e.Email).NotEmpty().WithMessage("Email Must Requered")
                                .EmailAddress().WithMessage("Email Must Be As Email Address");

            RuleFor(e => e.FullName).NotEmpty().WithMessage("Name Must Requered");

            RuleFor(e => e.PhoneNumber).NotEmpty().WithMessage("Phone Must Requered");


            //RuleFor(e => e.Types).NotEmpty().WithMessage("Must Chose Your Role");







        }
    }
}
