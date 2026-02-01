using PaymentService.Application.DTOs;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentSessionStore
    {
        void Save(PaymentSessionDto session);
        PaymentSessionDto? Get(string token);
        void Remove(string token);
    }
}
