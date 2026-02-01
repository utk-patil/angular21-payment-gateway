using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task<Transaction?> GetByOrderIdAsync(string orderId);
        public Task<PaginatedResponse<TransactionResponse>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? status);
        Task SaveChangesAsync();
    }
}
