using PaymentService.Application.DTOs;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<string> CreatePaymentAsync(string orderId, decimal amount);
        bool VerifySignature(string payload, string signature);
        Task SendWebhookAsync(PaymentWebhookDto payload);
    }
}
