using Microsoft.Extensions.Options;
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PaymentService.Infrastructure.Gateways
{
    public class DemoPaymentGateway : IPaymentGateway
    {
        private readonly PaymentOptions _options;
        private readonly HttpClient _http;

        public DemoPaymentGateway(IOptions<PaymentOptions> options, HttpClient http)
        {
            _options = options.Value;
            _http = http;
        }

        public Task<string> CreatePaymentAsync(string orderId, decimal amount)
        {
            var paymentUrl = $"{_options.PaymentUrl}/pay" + $"?orderId={orderId}" + $"&amount={amount}";

            return Task.FromResult(paymentUrl);
        }

        public bool VerifySignature(string payload, string signature)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.WebhookSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var computedSignature = Convert.ToHexString(hash);

            return computedSignature.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }

        public async Task SendWebhookAsync(PaymentWebhookDto payload)
        {
            var json = JsonSerializer.Serialize(payload);

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.WebhookSecret));

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(json));
            var signature = Convert.ToHexString(hash).ToLower();

            var request = new HttpRequestMessage(
                HttpMethod.Post, $"{_options.WebhookUrl}"
            );

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Headers.Add("X-Signature", signature);

            await _http.SendAsync(request);
        }
    }
}
