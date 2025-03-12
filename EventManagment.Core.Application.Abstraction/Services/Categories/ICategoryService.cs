using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Shared.Models.Categories;

namespace EventManagment.Core.Application.Abstraction.Services.Categories
{
    public interface ICategoryService
    {
        public Task<Response<CategoryDto>> CreateCategory(CategoryDto eventDto, CancellationToken cancellationToken);

    }
}
