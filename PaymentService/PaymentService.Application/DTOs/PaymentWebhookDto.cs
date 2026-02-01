namespace PaymentService.Application.DTOs
{
    public class PaymentWebhookDto
    {
        public string OrderId { get; set; } = null!;
        public string ProviderReference { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
