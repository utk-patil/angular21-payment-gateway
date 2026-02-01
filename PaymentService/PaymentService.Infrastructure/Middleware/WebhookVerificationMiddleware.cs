using Microsoft.AspNetCore.Http;
using PaymentService.Application.Interfaces;
using System.Text;

namespace PaymentService.Infrastructure.Middleware
{
    public class WebhookVerificationMiddleware
    {
        private readonly RequestDelegate _next;

        public WebhookVerificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPaymentGateway gateway)
        {
            if (!context.Request.Path.Value!.Contains("/webhook"))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);

            var payload = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var signature = context.Request.Headers["X-Signature"].ToString();

            if (!gateway.VerifySignature(payload, signature))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid webhook signature");
                return;
            }

            await _next(context);
        }
    }
}
