using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Core.Domain.Entities.Categories;
using EventManagment.Shared.Models.Categories;
using Microsoft.Extensions.Logging;

namespace EventManagment.Core.Application.Services.Categories
{
    public class CategoryService(IUnitOfWork _unitOfWork, ILogger<CategoryService> _logger, IMapper _mapper) : ResponseHandler, ICategoryService
    {
        public async Task<Response<CategoryDto>> CreateCategory(CategoryDto categoryDto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Create Category Service Called");

            var repo = _unitOfWork.GetRepository<Category, int>();

            var category = _mapper.Map<Category>(categoryDto);


            var addcategory = repo.AddAsync(category);
            if (addcategory is null)
            {
                _logger.LogWarning("Category Not Created");
                if (addcategory is null) return BadRequest<CategoryDto>("Operation No Successfuly");
            }

            var complete = await _unitOfWork.CompleteAsync() > 0;
            if (!complete) return BadRequest<CategoryDto>("Error Occure While Creating Category");
            var mappedresult = _mapper.Map<CategoryDto>(category);

            return Created(mappedresult);





        }

        public async Task<Response<CategoryDto>> GetCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Get Category By Id Service Called");



            var repo = _unitOfWork.GetRepository<Category, int>();
            var category = await repo.GetAsync(id);

            if (category is null)
            {
                _logger.LogWarning("Category Not Found With This Id");
                return NotFound<CategoryDto>(id, "Category Not Found With This Id");
            }

            var mappedCategory = _mapper.Map<CategoryDto>(category);
            _logger.LogInformation("Category Found And Mapped");

            return Success(mappedCategory, 1);




        }
    }
}
