using Microsoft.Extensions.Options;
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Options;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Services
{
    public class PaymentAppService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IPaymentSessionService _paymentSessionService;
        private readonly PaymentOptions _options;

        public PaymentAppService(ITransactionRepository transactionRepository, IPaymentGateway paymentGateway, IPaymentSessionService paymentSessionService, IOptions<PaymentOptions> options)
        {
            _transactionRepository = transactionRepository;
            _paymentGateway = paymentGateway;
            _paymentSessionService = paymentSessionService;
            _options = options.Value;
        }

        public async Task<string> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var transaction = await _transactionRepository.GetByOrderIdAsync(request.OrderId);

            if (transaction == null)
            {
                transaction = new Transaction
                {
                    OrderId = request.OrderId,
                    Amount = request.Amount,
                    Status = TransactionStatus.Created,
                    CreatedOn = DateTime.UtcNow
                };

                await _transactionRepository.AddAsync(transaction);
            }
            else
            {
                if (transaction.Status == TransactionStatus.Success)
                    throw new Exception("Payment already completed");

                transaction.Status = TransactionStatus.Created;
                transaction.UpdatedOn = DateTime.UtcNow;
            }

            var token = _paymentSessionService.CreateSession(transaction.OrderId, transaction.Amount);

            transaction.Status = TransactionStatus.Initiated;
            transaction.UpdatedOn = DateTime.UtcNow;

            await _transactionRepository.SaveChangesAsync();

            return $"{_options.PaymentUrl}?token={token}";
        }

        public async Task HandleWebhookAsync(PaymentWebhookDto dto)
        {
            var transaction = await _transactionRepository.GetByOrderIdAsync(dto.OrderId);

            if (transaction == null)
                throw new InvalidOperationException("Transaction not found");

            if (transaction.Status == TransactionStatus.Success)
                return;

            transaction.ProviderReference = dto.ProviderReference;
            transaction.Status = Enum.Parse<TransactionStatus>(dto.Status, true);
            transaction.UpdatedOn = DateTime.UtcNow;

            await _transactionRepository.SaveChangesAsync();
        }

        public async Task<PaginatedResponse<TransactionResponse>> GetTransactionsAsync(PaginationRequest request)
        {
            if (request.PageNumber <= 0)
                request.PageNumber = 1;

            if (request.PageSize <= 0)
                request.PageSize = 10;

            var result = await _transactionRepository.GetPagedAsync(request.PageNumber, request.PageSize, request.Search, request.Status);

            return result;
        }

        public async Task<string> ConfirmPaymentAsync(ConfirmPaymentRequest request)
        {
            var session = _paymentSessionService.GetSession(request.Token);

            var tx = await _transactionRepository.GetByOrderIdAsync(session.OrderId) ?? throw new Exception("Transaction not found");

            tx.Status = request.IsSuccess ? TransactionStatus.Success : TransactionStatus.Failed;

            if (string.IsNullOrEmpty(tx.ProviderReference))
            {
                tx.ProviderReference = GenerateProviderReference();
            }

            tx.Status = TransactionStatus.Processing;
            tx.UpdatedOn = DateTime.UtcNow;

            await _transactionRepository.SaveChangesAsync();

            //same application can't call own endpoint using http in same request
            var webhookPayload = new PaymentWebhookDto
            {
                OrderId = tx.OrderId,
                ProviderReference = tx.ProviderReference,
                Status = request.IsSuccess ? TransactionStatus.Success.ToString() : TransactionStatus.Failed.ToString()
            };

            //await _paymentGateway.SendWebhookAsync(webhookPayload);

            // calling webhook
            await HandleWebhookAsync(webhookPayload);

            _paymentSessionService.InvalidateSession(request.Token);

            return request.IsSuccess ? _options.ReturnUrl : _options.CancelUrl;
        }

        private static string GenerateProviderReference()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            var suffix = new string(Enumerable.Repeat(chars, 14)
                                .Select(s => s[random.Next(s.Length)])
                                .ToArray()
            );

            return $"pay_{suffix}";
        }
    }
}
