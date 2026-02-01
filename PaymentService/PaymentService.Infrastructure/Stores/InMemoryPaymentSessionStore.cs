using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using System.Collections.Concurrent;

namespace PaymentService.Infrastructure.Stores
{
    public class InMemoryPaymentSessionStore : IPaymentSessionStore
    {
        private readonly ConcurrentDictionary<string, PaymentSessionDto> _store = new();

        public void Save(PaymentSessionDto session)
        {
            _store[session.Token] = session;
        }

        public PaymentSessionDto? Get(string token)
        {
            _store.TryGetValue(token, out var session);
            return session;
        }

        public void Remove(string token)
        {
            _store.TryRemove(token, out _);
        }
    }
}
