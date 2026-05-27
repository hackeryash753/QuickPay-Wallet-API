# QuickPay Wallet API

A scalable digital wallet backend application built using ASP.NET Core Web API.

This project demonstrates modern backend engineering concepts including:
- JWT Authentication
- Wallet Management
- Money Transfer
- Transaction History
- Redis Caching
- RabbitMQ Messaging
- Background Workers
- Exception Middleware
- Database Transactions
- Pagination

---

# Tech Stack

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Redis Cache
- RabbitMQ
- Docker
- Swagger/OpenAPI

---

# Features

## Authentication
- User Registration
- User Login
- JWT Token Generation
- Protected APIs using JWT Authentication

## Wallet Management
- Create Wallet
- Check Wallet Balance
- Add Money
- Wallet Balance Cache using Redis

## Money Transfer
- Send Money Between Users
- Database Transactions for consistency
- Concurrency Handling

## Transaction History
- Paginated Transaction History
- Credit/Debit Tracking

## Notifications
- RabbitMQ Producer
- RabbitMQ Consumer
- Background Worker Service
- Notification Storage

## Error Handling
- Centralized Exception Middleware
- Custom Exception Handling
- Proper HTTP Status Codes

---

# Architecture

The application follows a layered architecture:

Controller Layer
↓
Service Layer
↓
Entity Framework Core / Database

Additional Components:
- Redis for caching
- RabbitMQ for asynchronous messaging
- BackgroundService for consumers
- Middleware for centralized exception handling

---

# Redis Caching

Wallet balance APIs are cached using Redis to reduce database calls and improve performance.

Cache invalidation is handled automatically during:
- Add Money
- Send Money

---

# RabbitMQ Messaging

RabbitMQ is used for asynchronous notification processing.

Flow:
1. Transaction Completed
2. Producer publishes message to queue
3. Consumer reads message
4. Notification stored in database

Dead Letter Queue (DLQ) handling is also implemented.

---

# API Features

- RESTful APIs
- JWT Authentication
- Pagination
- Exception Middleware
- Async Processing
- Database Transactions

---

# Setup Instructions

## Clone Repository

```bash
git clone https://github.com/hackeryash753/QuickPay-Wallet-API.git
