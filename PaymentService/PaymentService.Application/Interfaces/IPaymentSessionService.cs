using PaymentService.Application.DTOs;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentSessionService
    {
        string CreateSession(string orderId, decimal amount);
        PaymentSessionDto GetSession(string token);
        void InvalidateSession(string token);
    }
}
