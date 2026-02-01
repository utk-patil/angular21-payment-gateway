using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;

namespace PaymentService.Application.Services
{
    public class PaymentSessionService : IPaymentSessionService
    {
        private readonly IPaymentSessionStore _store;

        public PaymentSessionService(IPaymentSessionStore store)
        {
            _store = store;
        }

        public string CreateSession(string orderId, decimal amount)
        {
            var token = Guid.NewGuid().ToString("N");

            _store.Save(new PaymentSessionDto
            {
                Token = token,
                OrderId = orderId,
                Amount = amount,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            });

            return token;
        }

        public PaymentSessionDto GetSession(string token)
        {
            var session = _store.Get(token);

            if (session == null || session.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired payment session");

            return session;
        }

        public void InvalidateSession(string token)
        {
            _store.Remove(token);
        }
    }

}
