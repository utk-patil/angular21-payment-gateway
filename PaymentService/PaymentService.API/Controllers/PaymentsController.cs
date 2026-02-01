using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentSessionService _paymentSessionService;
        private readonly PaymentAppService _paymentAppService;

        public PaymentsController(IPaymentSessionService paymentSessionService, PaymentAppService paymentAppService)
        {
            _paymentSessionService = paymentSessionService;
            _paymentAppService = paymentAppService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var paymentUrl = await _paymentAppService.CreatePaymentAsync(request);

            return Ok(new
            {
                request.OrderId,
                paymentUrl
            });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] PaginationRequest request)
        {
            var result = await _paymentAppService.GetTransactionsAsync(request);

            return Ok(result);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment(ConfirmPaymentRequest request)
        {
            var redirectUrl = await _paymentAppService.ConfirmPaymentAsync(request);

            return Ok(new { redirectUrl });
        }

        [HttpGet("demo-pay/session/{token}")]
        public IActionResult GetDemoPaySession(string token)
        {
            try
            {
                var session = _paymentSessionService.GetSession(token);

                return Ok(new
                {
                    orderId = session.OrderId,
                    amount = session.Amount
                });
            }
            catch
            {
                return Unauthorized("Invalid or expired payment session");
            }
        }

    }
}
