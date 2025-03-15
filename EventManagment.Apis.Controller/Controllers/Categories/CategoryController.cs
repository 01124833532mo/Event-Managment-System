using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Categories
{
    [Authorize]
    public class CategoryController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDto categorydto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.CategoryService.CreateCategory(categorydto, cancellationToken);
            return NewResult(result);

        }
        [AllowAnonymous]
        [HttpGet("GetCategory/{id}")]
        public async Task<ActionResult> GetCategory([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.CategoryService.GetCategoryAsync(id, cancellationToken);
            return NewResult(result);

        }

        [AllowAnonymous]
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<Pagination<CategoryDto>>> GetAllEvents([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.CategoryService.GetAllCategoriesAsynce(specParams, cancellationToken);
            return Ok(products);
        }
    }
}
