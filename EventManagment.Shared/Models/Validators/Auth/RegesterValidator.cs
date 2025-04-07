using EventManagment.Shared.Models._Common;
using EventManagment.Shared.Models.Auth;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.Auth
{
    public class RegesterValidator : AbstractValidator<RegisterDto>
    {
        public RegesterValidator()
        {
            RuleFor(e => e.Email).NotEmpty().WithMessage("Email Must Requered")
                                .EmailAddress().WithMessage("Email Must Be As Email Address");

            RuleFor(e => e.FullName).NotEmpty().WithMessage("Name Must Requered");

            RuleFor(x => x.PhoneNumber)
                          .NotEmpty()
                          .WithMessage("PhoneNumber Must Not Empty , Plz Add a {PropertyName}")
                          .Matches(RegexPatterns.PhoneNumber).WithMessage("Invalid Egyptian phone number.");


            RuleFor(x => x.Password)
                           .NotEmpty()
                           .WithMessage("\"Password Must Not Empty ,Plz Add a {PropertyName}\"")
                           .Matches(RegexPatterns.Password).WithMessage("Password must be at least 8 characters long and contain at least one digit.");



            RuleFor(x => x.Age)
                 .GreaterThanOrEqualTo(18)
                    .When(x => x.Age.HasValue)
                     .WithMessage("Age must be at least 18 ");


            RuleFor(x => x.Address)
              .NotEmpty().When(x => x.Address != null)
              .WithMessage("Address cannot be empty if provided")
              .MaximumLength(200).WithMessage("Address cannot be longer than 200 characters");


            RuleFor(x => x.CompanyName)
             .NotEmpty().When(x => x.CompanyName != null)
             .WithMessage("Company name cannot be empty if provided")
             .MaximumLength(100).WithMessage("Company name cannot be longer than 100 characters");


            RuleFor(x => x.Types).NotNull().WithMessage("Type Must Requered");

        }
    }
}
