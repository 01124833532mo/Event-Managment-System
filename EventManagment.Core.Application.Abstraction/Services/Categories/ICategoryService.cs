using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Categories;

namespace EventManagment.Core.Application.Abstraction.Services.Categories
{
    public interface ICategoryService
    {
        public Task<Response<CategoryDto>> CreateCategory(CategoryDto eventDto, CancellationToken cancellationToken);

        public Task<Response<CategoryDto>> GetCategoryAsync(int id, CancellationToken cancellationToken);

        Task<Pagination<CategoryDto>> GetAllCategoriesAsynce(SpecParams specParams, CancellationToken cancellationToken);


    }
}
