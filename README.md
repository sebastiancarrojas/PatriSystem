# PatriSystem API

> Modular REST API for small retail store management — built with Clean Architecture and .NET 8.

---

## Overview

**PatriSystem** is a backend API designed to manage the day-to-day operations of a small retail store. It is built with scalability and modularity in mind, allowing new business modules to be added independently without affecting the core system.

This project serves as a real-world application of Clean Architecture principles in a .NET 8 environment.

---

## Modules

| Module | Description | Status |
|--------|-------------|--------|
| **POS** | Point of Sale — products, sales, inventory | 🚧 In progress |
| **Fiados** | Credit tracking for trusted customers | 📋 Planned |
| **Separados** | Layaway / payment plan management | 📋 Planned |

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | .NET 8 / ASP.NET Core Web API |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Mapping | AutoMapper |
| Auth | JWT Bearer Authentication |
| Docs | Swagger |

---

## Architecture

PatriSystem follows **Clean Architecture** with a clear separation of concerns across three projects:

```
PatriSystem/
├── PatriSystem.API/              # Presentation layer
│   ├── Controllers/              # REST endpoints
│   ├── DTOs/
│   │   ├── Request/              # Input models
│   │   └── Response/             # Output models
│   ├── Mappings/                 # AutoMapper profiles
│   └── Middlewares/              # Error handling, auth
│
├── PatriSystem.Domain/           # Business logic layer
│   ├── Entities/                 # Core business models
│   ├── Enums/                    # Domain enumerations
│   ├── Interfaces/
│   │   ├── Repositories/         # Data access contracts
│   │   └── Services/             # Business logic contracts
│   └── Services/                 # Business logic implementations
│
└── PatriSystem.DataAccess/       # Data layer
    ├── Context/                  # EF Core DbContext
    ├── Repositories/             # Repository implementations
    └── Migrations/               # EF Core migrations (auto-generated)
```

### Dependency Flow

```
API → Domain ← DataAccess
API → DataAccess (for DI registration)
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or SQL Server Express)
- [EF Core CLI Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/your-username/PatriSystem.git
cd PatriSystem
```

2. **Configure the database connection**

Edit `PatriSystem.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PatriSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

3. **Apply migrations**
```bash
cd PatriSystem.API
dotnet ef database update
```

4. **Run the API**
```bash
dotnet run
```

The API will be available at `https://localhost:5001` and Swagger at `https://localhost:5001/swagger`.

---

## API Documentation

Once the project is running, full interactive documentation is available via Swagger UI at `/swagger`.

---

## Roadmap

- [x] Project setup with Clean Architecture
- [ ] POS module — products and categories
- [ ] POS module — sales and transactions
- [ ] JWT Authentication & role-based access
- [ ] Fiados module
- [ ] Separados module

---

## License

This project is licensed under the MIT License.
