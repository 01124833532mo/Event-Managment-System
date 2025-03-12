using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
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
    }
}
