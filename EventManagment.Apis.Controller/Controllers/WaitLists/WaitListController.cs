using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
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

    }
}
