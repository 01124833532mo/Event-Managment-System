using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction;
using EventManagment.Shared.Models.Registrations;
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

    }
}
