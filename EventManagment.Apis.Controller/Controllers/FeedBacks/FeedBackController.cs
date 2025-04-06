using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models.FeedBacks;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.FeedBacks
{
    [Authorize]
    public class FeedBackController(IServiceManager serviceManager) : BaseApiController
    {
        [Authorize(Roles = Roles.Attendee)]
        [HttpPost("CreateFeedBack")]
        public async Task<ActionResult> CreateFeedBack([FromBody] CreateFeedBackDto createFeedBackDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.FeedBackService.CreateFeedBack(User, createFeedBackDto, cancellationToken);
            return NewResult(result);

        }

    }
}
