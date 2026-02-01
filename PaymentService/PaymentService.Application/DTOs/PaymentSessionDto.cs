namespace PaymentService.Application.DTOs
{
    public class PaymentSessionDto
    {
        public string Token { get; set; } = default!;
        public string OrderId { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
