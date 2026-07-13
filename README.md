# 💳 QuickPay Wallet API

> A production-ready digital wallet backend built with **ASP.NET Core 8**, featuring JWT Authentication, SQL Server, Redis Caching, RabbitMQ Messaging, and Docker Compose.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-5C2D91?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Authentication-black?logo=jsonwebtokens)
![License](https://img.shields.io/badge/License-MIT-green)

---

## 📖 Overview

QuickPay is a secure digital wallet backend that allows users to:

- Register and authenticate using JWT
- Add money to wallet
- Send money to other users
- View wallet balance
- View paginated transaction history
- Receive asynchronous notifications
- Improve performance using Redis cache
- Run the complete application using Docker Compose

---

# 🏗️ Architecture

![Architecture](Docs/architecture.png)


---

# 🚀 Features

- ✅ JWT Authentication & Authorization
- ✅ Secure Password Hashing
- ✅ Wallet Management
- ✅ Money Transfer
- ✅ Add Money
- ✅ Transaction History with Pagination
- ✅ Redis Distributed Cache
- ✅ RabbitMQ Message Queue
- ✅ Background Consumer Service
- ✅ Notification System
- ✅ SQL Server Database
- ✅ Entity Framework Core
- ✅ Dockerized Application
- ✅ Docker Compose
- ✅ RESTful APIs
- ✅ Swagger Documentation

---

# 🛠 Tech Stack

| Technology | Purpose |
|------------|---------|
| ASP.NET Core 8 | REST API |
| Entity Framework Core | ORM |
| SQL Server | Database |
| Redis | Distributed Cache |
| RabbitMQ | Message Broker |
| JWT | Authentication |
| Docker | Containerization |
| Docker Compose | Multi-container orchestration |

---

# 📂 Project Structure

```
QuickPay
│
├── Controllers/
├── Services/
├── Models/
│   ├── Domain/
│   └── DTO/
├── Data/
├── Repository/
├── Migrations/
├── Dockerfile
├── docker-compose.yml
└── Program.cs
```

---

# 🔄 System Flow

```
Client
   │
   ▼
ASP.NET Core API
   │
   ├────────► SQL Server
   │
   ├────────► Redis Cache
   │
   └────────► RabbitMQ
                     │
                     ▼
           Background Consumer
                     │
                     ▼
             Notifications Table
```

---

# 🐳 Running with Docker

Clone the repository

```bash
git clone https://github.com/hackeryash753/QuickPay-Wallet-API.git
```

Navigate to project

```bash
cd QuickPay-Wallet-API
```

Build containers

```bash
docker compose build
```

Run containers

```bash
docker compose up -d
```

Stop containers

```bash
docker compose down
```

---

# 🔑 API Endpoints

## Authentication

| Method | Endpoint |
|---------|----------|
| POST | /api/auth/register |
| POST | /api/auth/login |

---

## Wallet

| Method | Endpoint |
|---------|----------|
| POST | /api/wallet/add-money |
| POST | /api/wallet/send-money |
| GET | /api/wallet/balance |
| GET | /api/wallet/transactions |

---

# 📸 API Screenshots

## Swagger UI
![Swagger](Docs/swagger-home.png)


---

## Register API

![Register](Docs/register-api.png)

---

## Login API

![Login](Docs/login-api.png)

---

## Add Money API

![Add Money](Docs/add-money.png)

---

## Send Money API

![Send Money](Docs/send-money.png)

---

# ⚡ Highlights

- Redis caching for wallet balance
- RabbitMQ asynchronous notifications
- Retry mechanism for RabbitMQ consumer
- Dead Letter Queue support
- JWT secured endpoints
- Dockerized development environment
- SQL Server persistence

---

# 📈 Future Improvements

- Refresh Tokens
- Email Notifications
- Rate Limiting
- API Versioning
- Serilog Logging
- Health Checks
- CI/CD Pipeline
- Azure Deployment
- Unit Testing
- Integration Testing

---

# 👨‍💻 Author

**Yash Jain**

Backend Developer

- ASP.NET Core
- C#
- SQL Server
- Docker
- RabbitMQ
- Redis

---

⭐ If you found this project useful, consider giving it a star!
