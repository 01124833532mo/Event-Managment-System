using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Common;
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

        [Authorize(Roles = Roles.Attendee)]
        [HttpDelete("RemoveFeedBack/{id}")]
        public async Task<ActionResult> RemoveFeedBack([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.FeedBackService.RemoveFeedBack(id, cancellationToken);
            return NewResult(result);
        }
        [AllowAnonymous]
        [HttpGet("GetAllFeedBack")]
        public async Task<ActionResult<Pagination<FeedBackToRetuen>>> GetAllFeedBacks([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.FeedBackService.GetAllFeedBack(specParams, cancellationToken);
            return Ok(products);
        }
        [AllowAnonymous]
        [HttpGet("GetFeedBackById/{id}")]
        public async Task<ActionResult> GetFeedBackById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.FeedBackService.GetFeedBackByIdAsync(id, cancellationToken);
            return NewResult(result);

        }
        [Authorize(Roles = Roles.Attendee)]
        [HttpGet("Get-FeedBack-To-Specific-Attendde")]
        public async Task<ActionResult<Pagination<FeedBackToRetuen>>> GetAllFeedBackForSpecificAttendee([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.FeedBackService.GetAllFeedBackForSpecificAttendee(User, specParams, cancellationToken);
            return Ok(products);
        }
    }
}
