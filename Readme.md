# Task Subscription System

A lightweight subscription management system built with **.NET 8**, following **Clean Architecture** principles. This project allows users to browse plans, activate subscriptions, and manage their subscription lifecycle.

---

## 🧠 Why Clean Architecture?

This project adopts **Clean Architecture** to achieve:

- **Separation of Concerns**: Clear boundaries between presentation, application logic, domain models, and infrastructure.
- **Testability**: Business logic is decoupled from frameworks and databases, making unit and integration testing straightforward.
- **Maintainability**: Changes in one layer (e.g., switching from SQL Server to PostgreSQL) have minimal impact on others.
- **Scalability**: Core business rules remain stable even as external concerns (UI, DB, APIs) evolve.
- **Dependency Rule**: Dependencies flow inward—from infrastructure and UI toward the domain—ensuring the core logic remains pure and framework-agnostic.

This structure makes the system easier to understand, extend, and onboard new developers.

---

## 🚀 Future Improvements for Large-Scale Deployment

If this system were to be scaled for production with thousands of users, the following enhancements would be implemented:

1. **Authentication & Identity**  
   - Integrate **OpenID Connect** with **Google OAuth** (and potentially other providers) using **Microsoft Identity Web** or **Auth0**.
   - Replace basic user context with a robust identity system (e.g., ASP.NET Core Identity with external logins).

2. **Payment Integration**  
   - Add support for **Stripe**, **PayPal**, or **local payment gateways**.
   - Implement webhook handlers for payment success/failure, subscription renewal, and cancellation.
   - Store payment metadata securely and comply with **PCI-DSS** standards.

3. **Performance Optimization**  
   - Use **Redis** as a distributed cache to store frequently accessed data (e.g., active plan list), reducing database load and improving response times.
   - Implement **pagination**, **rate limiting**, and **caching headers** in APIs.

4. **Additional Features**  
   - Subscription downgrade/upgrade with prorated billing.
   - Usage-based billing and metered plans.
   - Admin dashboard for plan and subscription management.
   - Background services for expiration alerts and auto-renewal.

---

## ⏱️ Development Time

This task was completed in **2.5 hours** (two hours and thirty minutes).

---

## 🤖 AI Assistance Disclosure

Artificial Intelligence (AI) tools were used to assist in the following areas:

- Generating **DTOs**, **entity models**, and **mapping logic**.
- Creating **Fluent API configurations** (`IEntityConfigured` classes) for Entity Framework Core.
- Drafting and formatting this **README.md** file.

All AI-generated code was **reviewed, validated, and adapted** to fit the project’s architecture, naming conventions, and business requirements. Core logic and architectural decisions were made manually.

---

## 🛠️ Technologies Used

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Clean Architecture** (Domain, Application, Infrastructure, API layers)
- **C# 12**
- **JWT-based Authentication** (current) → *to be extended with OAuth*

---

> ✨ This project demonstrates a solid foundation for a subscription-based SaaS product, ready to evolve with real-world demands.