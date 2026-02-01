using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Services;

namespace PaymentService.Webhook.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly PaymentAppService _paymentAppService;

        public WebhookController(PaymentAppService paymentAppService)
        {
            _paymentAppService = paymentAppService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> Webhook([FromBody] PaymentWebhookDto dto)
        {
            await _paymentAppService.HandleWebhookAsync(dto);
            return Ok();
        }
    }
}
