using EventManagment.Apis.Controller.Controllers.Base;
using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagment.Apis.Controller.Controllers.Pyments
{
    public class PaymentController(IPaymentService paymentService) : BaseApiController
    {
        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {


            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();


            await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);
            return Ok();
        }

    }
}
