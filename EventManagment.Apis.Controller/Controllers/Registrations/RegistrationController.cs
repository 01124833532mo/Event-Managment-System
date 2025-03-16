using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Core.Application.Abstraction.Common;
using EventManagment.Shared.Models.Registrations;
using EventManagment.Shared.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Registrations
{
    [Authorize]
    public class RegistrationController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("CreateRegistration")]
        public async Task<ActionResult> CreateRegistraction([FromBody] CreateRegisterDto createRegisterDto, CancellationToken cancellationToken)
        {
            var result = await serviceManager.RegistrationService.CreateRegisterAsync(createRegisterDto, cancellationToken);
            return NewResult(result);

        }

        [AllowAnonymous]
        [HttpGet("GetRegistrationById/{id}")]
        public async Task<ActionResult> GetRegistrationById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await serviceManager.RegistrationService.GetRegistrationAsync(id, cancellationToken);
            return NewResult(result);

        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllRegistrations-For-Admin")]
        public async Task<ActionResult<Pagination<RegisterToReturn>>> GetAllRegistration([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.RegistrationService.GetAllRegistrationsAsync(specParams, cancellationToken);
            return Ok(products);
        }
        [HttpGet("GetAll-Registration-For-Specificg-Attendd")]
        public async Task<ActionResult<Pagination<RegisterToReturn>>> GetAllRegistrationForSpecificgAttendd([FromQuery] SpecParams specParams, CancellationToken cancellationToken)
        {
            var products = await serviceManager.RegistrationService.GetAllRegistrationForSpecificUserAsync(specParams, User, cancellationToken);
            return Ok(products);
        }

        [HttpDelete("Cancel-Registration/{id}")]
        public async Task<ActionResult<string>> CancelRegistration([FromRoute] int id, CancellationToken cancellationToken)
        {
            var products = await serviceManager.RegistrationService.CancelRegistrationAsync(id, cancellationToken);
            return Ok(products);
        }

    }
}
