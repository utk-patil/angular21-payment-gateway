namespace PaymentService.Application.DTOs
{
    public class ConfirmPaymentRequest
    {
        public string Token { get; set; } = default!;
        public bool IsSuccess { get; set; }
    }
}
