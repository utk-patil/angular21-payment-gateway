# Payment Gateway Demo – Clean Architecture

A secure, retry-enabled payment gateway demo built with **ASP.NET Core (.NET 10)** and **Angular 21**, following **Clean Architecture principles**.  
This project demonstrates real-world payment flows such as **Create Payment**, **Repay**, **secure token-based redirection**, and **webhook simulation**.

---

## 🚀 Key Features

- Clean Architecture (Domain, Application, Infrastructure, API)
- Secure token-based payment session (no sensitive data in URLs)
- Create Payment & Repay using the same API
- Demo payment gateway page
- Retry (Repay) flow with idempotent backend logic
- Webhook simulation with signature verification
- Transaction listing with:
  - Pagination
  - Search (Order ID / Provider Reference)
  - Status filtering
- Angular 21 Signals for state management
- UTC timestamps with frontend local-time conversion
- Reference SQL schema for future database setup

---

## 🏗️ Architecture Overview

The solution follows **Clean Architecture**:

```
PaymentService.Domain
  └── Core business entities and enums

PaymentService.Application
  └── Use cases, DTOs, interfaces, and application services

PaymentService.Infrastructure
  └── EF Core, repositories, in-memory session store, gateway implementations

PaymentService.Api
  └── Controllers, middleware, configuration

payment-ui (Angular 21)
  └── Standalone components, signals, services, and pages
```

### Design Principles
- API depends on **interfaces**, not implementations
- Backend is the **single source of truth**
- Frontend never trusts URL parameters for payment data
- Retry logic is centralized in the backend

---

## 🔐 Secure Payment Flow

### Create Payment / Repay Flow

1. User clicks **Pay** or **Repay**
2. Angular calls `CreatePayment` API
3. Backend:
   - Creates or reuses a transaction
   - Generates a **short-lived payment session token**
4. Backend returns a **secure payment URL**
5. Browser redirects to **Demo Pay Page**
6. Demo Pay page:
   - Calls backend using token
   - Fetches trusted order details
7. User confirms or cancels payment
8. Backend:
   - Validates token & expiry
   - Updates transaction
   - Triggers webhook
   - Invalidates token

> No order ID or amount is ever trusted from the URL.

---

## 🔁 Repay Logic

- Repay uses the **same Create Payment API**
- No separate Repay endpoint
- Backend determines retry vs new payment
- Amount is always taken from the database
- Successful payments cannot be retried

---

## 🔔 Webhook Simulation

- Webhook payload includes:
  - Order ID
  - Provider Reference
  - Payment Status
- Signature verification using shared secret
- Simulates real payment provider behavior

---

## 🗄️ Database Reference

```
PaymentService.Api/Database/Tables/transactions.sql
```

This file contains a **reference SQL schema** for the `Transactions` table.  
It is provided for documentation and future database setup purposes only.

---

## 🖥️ Frontend (Angular 21)

- Standalone components
- Angular Signals for state management
- Modern template syntax (`@if`, `@for`)
- Secure demo payment page
- Clean UI with consistent theming
- Pagination, filters, and retry actions

---

## ⚙️ How to Run

### Backend (ASP.NET Core)
```bash
cd PaymentService.Api
dotnet run
```

### Frontend (Angular)
```bash
cd payment-ui
npm install
ng serve
```

### URLs
```
Angular App:   http://localhost:4200
API Swagger:  http://localhost:7043/swagger
Webhook Swagger:  http://localhost:7296/swagger
```

---

## 📌 Notes

- This is a **demo project** – no real payments are processed
- Token-based flow mirrors real gateways like Stripe / Razorpay
- Designed for **learning, interviews, and assignments**
- Focuses on correctness, security, and clean architecture

---

## 📄 License

This project is for educational and demonstration purposes only.
