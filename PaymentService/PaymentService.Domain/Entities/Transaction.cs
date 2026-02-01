using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public string OrderId { get; set; } = null!;
        public string? ProviderReference { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
