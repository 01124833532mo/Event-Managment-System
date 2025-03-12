using EventManagment.Shared.Models.Categories;
using FluentValidation;

namespace EventManagment.Shared.Models.Validators.Categories
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {

        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                                .NotNull().WithMessage("Name Must be Not Null");
        }
    }
}
