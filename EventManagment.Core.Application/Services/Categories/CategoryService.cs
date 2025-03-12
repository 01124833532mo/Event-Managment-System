using AutoMapper;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Services.Categories;
using EventManagment.Core.Domain.Contracts.Persestence;
using EventManagment.Shared.Models.Categories;
using Microsoft.Extensions.Logging;

namespace EventManagment.Core.Application.Services.Categories
{
    public class CategoryService(IUnitOfWork _unitOfWork, ILogger<CategoryService> _logger, IMapper _mapper) : ICategoryService
    {
        public Task<Response<CategoryDto>> CreateEvent(CategoryDto eventDto)
        {
            throw new NotImplementedException();
        }
    }
}
