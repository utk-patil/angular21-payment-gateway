using Microsoft.EntityFrameworkCore;
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Infrastructure.Data;

namespace PaymentService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;

        public TransactionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _db.Transactions.AddAsync(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task<Transaction?> GetByOrderIdAsync(string orderId)
        {
            return await _db.Transactions.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<PaginatedResponse<TransactionResponse>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? status)
        {
            var query = _db.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.OrderId.Contains(search) || (t.ProviderReference != null && t.ProviderReference.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(t => t.Status.ToString().ToLower() == status.ToLower());
            }

            var totalRecords = await query.CountAsync();

            var data = await query.OrderByDescending(t => t.UpdatedOn)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PaginatedResponse<TransactionResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = data.Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    OrderId = t.OrderId,
                    ProviderReference = t.ProviderReference,
                    Amount = t.Amount,
                    Status = t.Status.ToString(),
                    UpdatedOn = t.UpdatedOn
                }).ToList()
            };
        }


        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
