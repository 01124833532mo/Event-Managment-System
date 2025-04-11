using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Bases;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Roles;
using EventManagment.Shared.Models.WaitList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.WaitLists
{

    [Authorize(Roles = Roles.Admin)]
    public class WaitListController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet("GetAllInWaitList")]
        public async Task<ActionResult<Pagination<WaitListToReturn>>> GetAllInWaitList([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.WaitListService.GetWalitListAsync(specParams, cancellationToken);
            return Ok(products);
        }
        [HttpGet("GetWaitList/{id}")]
        public async Task<ActionResult<Response<WaitListToReturn>>> GetWaitList([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.WaitListService.GetWaitListByIdAsync(id, cancellationToken);
            return NewResult(result);
        }

    }
}
