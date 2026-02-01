using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Options;
using PaymentService.Application.Services;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.Gateways;
using PaymentService.Infrastructure.Middleware;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<PaymentOptions>(builder.Configuration.GetSection("Payment"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IPaymentGateway, DemoPaymentGateway>();

builder.Services.AddScoped<PaymentAppService>();

builder.Services.AddSingleton<IPaymentSessionStore, InMemoryPaymentSessionStore>();
builder.Services.AddSingleton<IPaymentSessionService, PaymentSessionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseMiddleware<WebhookVerificationMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
