namespace PaymentService.Application.DTOs
{
    public class TransactionResponse
    {
        public long Id { get; set; }
        public string OrderId { get; set; } = null!;
        public string? ProviderReference { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? UpdatedOn { get; set; }
    }
}
