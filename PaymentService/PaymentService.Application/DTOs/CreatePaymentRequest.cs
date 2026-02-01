namespace PaymentService.Application.DTOs
{
    public class CreatePaymentRequest
    {
        public string OrderId { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
