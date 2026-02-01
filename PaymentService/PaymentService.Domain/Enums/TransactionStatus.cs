namespace PaymentService.Domain.Enums
{
    public enum TransactionStatus
    {
        Created = 0,
        Initiated = 1,
        Success = 2,
        Failed = 3,
        Processing = 4,
        Cancelled = 5,
    }
}
