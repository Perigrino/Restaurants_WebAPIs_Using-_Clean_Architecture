# Restaurants Web API

A RESTful web API for exploring restaurants and their dishes. It provides structured data such as restaurant categories, delivery availability, contact details, addresses, and menu items — useful for food delivery platforms, restaurant directories, market research, and local dining guides.

Built with ASP.NET Core 10, Entity Framework Core (PostgreSQL), ASP.NET Core Identity with cookie authentication, and Serilog for logging.

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Setup](#database-setup)
  - [Configuration](#configuration)
  - [Running the API](#running-the-api)
- [Authentication](#authentication)
- [API Reference](#api-reference)
  - [Identity](#identity)
  - [Restaurants](#restaurants)
  - [Dishes](#dishes)
  - [Users](#users)
- [Response Envelope](#response-envelope)
- [Error Handling](#error-handling)
- [Migrations](#migrations)
- [License](#license)

## Features

- CRUD operations for Restaurants and Dishes
- Relational model: a User owns many Restaurants; a Restaurant has many Dishes
- ASP.NET Core Identity with three roles: `User`, `Owner`, and `Administrator`
- Automatic database migration and data seeding on startup
- Data validation on all request bodies
- Case-insensitive search, pagination, and sorting for restaurant listings
- Policy-based authorization: nationality, minimum age, and restaurant-ownership requirements
- Consistent JSON response envelope
- Centralized exception handling returning proper HTTP status codes
- Swagger/OpenAPI documentation
- Structured console + rolling file logging with Serilog

## Tech Stack

| Layer        | Technology                                   |
| ------------ | -------------------------------------------- |
| Runtime      | .NET 10                                      |
| Web framework| ASP.NET Core (minimal hosting)               |
| ORM          | Entity Framework Core 10 + Npgsql            |
| Database     | PostgreSQL                                   |
| Auth         | ASP.NET Core Identity (cookie)               |
| CQRS         | MediatR 14                                   |
| Mapping      | AutoMapper 16                                |
| Validation   | FluentValidation 11                          |
| API docs     | Swashbuckle / Swagger                        |
| Logging      | Serilog                                      |

## Project Structure

```
Restaurants_WebAPIs_Using-_Clean_Architecture/
├── Restaurants.API/                    # ASP.NET Core web API (entry point)
│   ├── Controller/                     # Restaurant, Dish, Identity controllers
│   ├── Extensions/                     # AddPresentation (Swagger, MVC, Serilog)
│   ├── Middlewares/                    # ErrorHandling, RequestTimeLogging
│   ├── Program.cs                      # App composition & pipeline
│   └── appsettings.json
├── Restaurants.Application/            # Use cases, DTOs, validators
│   ├── Common/                         # PageResults, FinalResponse
│   ├── Dishes/                         # Commands, queries, DTOs, validators
│   ├── Restaurants/                    # Commands, queries, DTOs, validators
│   ├── Users/                          # CurrentUser, UserContext
│   └── Extensions/                     # AddApplication (MediatR, AutoMapper, FluentValidation)
├── Restaurants.Domain/                 # Core entities, interfaces, constants
│   ├── Constants/                      # UserRoles, ResourceOperation, SortDirection
│   ├── Entities/                       # Restaurant, Dish, User, Address
│   ├── Exceptions/                     # NotFoundException, ForbiddenException
│   └── IRepository/                    # Repository + authorization interfaces
├── Restaurants.Infrastructure/         # Persistence, repositories, seeder, auth
│   ├── Authorisation/                  # Policies, requirements, claims factory
│   ├── Migrations/                     # EF Core migrations
│   ├── Persistence/                    # RestaurantDbContext + design-time factory
│   ├── Repositories/                   # Restaurant, Dish repositories
│   ├── Seeder/                         # Database seeder
│   └── Extensions/                     # AddInfrastructureService (EF, Identity, policies)
├── Restaurants.Tests/                  # xUnit tests (validators, mappings, auth, repos)
└── Restaurants.sln
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- PostgreSQL (local or remote instance)

### Database Setup

The API ships with an EF Core migration (`InitialCreate`) and **applies it automatically on startup**. To apply it manually:

```bash
dotnet ef database update --project Restaurants.Infrastructure --startup-project Restaurants.API
```

On startup the seeder also runs: if the tables are empty it inserts the `User`, `Owner`, and `Administrator` roles plus two sample restaurants (KFC and McDonald's) with dishes.

### Configuration

The connection string is **not** stored in `appsettings.json`. Set it via user-secrets (or an environment variable):

```bash
dotnet user-secrets set "ConnectionStrings:Default" "User ID=postgres;Password=123;Host=localhost;Port=5432;Database=Restaurants_Db;Pooling=true;" --project Restaurants.API
```

Logging (Serilog) is configured in `appsettings.json`:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": { "outputTemplate": "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine}{Message:lj}{NewLine}{Exception}" }
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/Restaurant-Api-.log",
          "rollingInterval": "Day",
          "rollOnFileSizeLimit": true,
          "formatter": "Serilog.Formatting.Compact.CompactJsonFormatter, Serilog.Formatting.Compact"
        }
      }
    ]
  }
}
```

### Running the API

```bash
dotnet run --project Restaurants.API
```

The API listens on:

- HTTP: http://localhost:5230
- HTTPS: https://localhost:7287
- Swagger UI: http://localhost:5230/swagger

## Authentication

The API uses **ASP.NET Core Identity** with cookie-based authentication, exposed through the standard Identity API endpoints at `/api/identity`.

1. **Register a user**

```http
POST /api/identity/register
Content-Type: application/json

{
  "email": "jane@example.com",
  "password": "password123"
}
```

2. **Log in**

```http
POST /api/identity/login
Content-Type: application/json

{
  "email": "jane@example.com",
  "password": "password123"
}
```

Login sets an authentication cookie (and returns an access token in the response body). Include the cookie in subsequent requests:

```
Cookie: .AspNetCore.Identity.Application=<cookie-value>
```

### Roles

Three roles are seeded automatically on startup:

| Role            | Description                                                      |
| --------------- | ---------------------------------------------------------------- |
| `User`          | Default role; can access authenticated endpoints                 |
| `Owner`         | Can create restaurants and update/delete their own               |
| `Administrator` | Can assign/remove roles and delete any restaurant                |

### Policies

| Policy                       | Requirement                                                |
| ---------------------------- | ---------------------------------------------------------- |
| `HasNationality`             | User holds a `Nationality` claim of `Ghanaian` or `Brazilian` |
| `AtLeast20`                  | User is at least 20 years old                               |
| `CreatedAtLeast2Restaurants` | User has created 2 or more restaurants                      |

## API Reference

All responses use the standard envelope (see [Response Envelope](#response-envelope)). A `Location` header is returned on creation.

### Identity

The standard ASP.NET Core Identity endpoints are exposed at `/api/identity`:

| Method | Endpoint                      | Description            |
| ------ | ----------------------------- | ---------------------- |
| POST   | `/api/identity/register`      | Register a new user    |
| POST   | `/api/identity/login`         | Log in (sets cookie)   |
| POST   | `/api/identity/refresh`       | Refresh authentication |
| POST   | `/api/identity/logout`        | Log out                |
| POST   | `/api/identity/manage/info`   | Manage account info    |
| POST   | `/api/identity/manage/2fa`    | Manage two-factor auth |

### Restaurants

| Method | Endpoint               | Auth                       | Description          |
| ------ | ---------------------- | -------------------------- | -------------------- |
| GET    | `/api/Restaurant`      | Anonymous                  | List restaurants     |
| GET    | `/api/Restaurant/{id}` | Policy: `HasNationality`   | Get a restaurant     |
| POST   | `/api/Restaurant`      | Role: `Owner`              | Create a restaurant  |
| PUT    | `/api/Restaurant/{id}` | Owner or Administrator     | Update a restaurant  |
| DELETE | `/api/Restaurant/{id}` | Owner or Administrator     | Delete a restaurant  |

Create / Update restaurant body:

```json
{
  "name": "KFC",
  "description": "American fast food chain specializing in fried chicken.",
  "category": "Fast Food",
  "hasDelivery": true,
  "contactEmail": "contact@kfc.com",
  "contactNumber": "+123456789",
  "city": "London",
  "street": "Cork St 5",
  "postalCode": "12-345"
}
```

Query parameters for `GET /api/Restaurant`:

| Parameter       | Type     | Description                                        |
| --------------- | -------- | -------------------------------------------------- |
| `searchPhrase`  | string?  | Case-insensitive match on name or description      |
| `pageNumber`    | int      | 1-based page number (default 1)                    |
| `pageSize`      | int      | One of: 5, 10, 15, 20, 25, 30, 35, 40, 45         |
| `sortBy`        | string?  | `Name`, `Category`, or `Description`               |
| `sortDirection` | int      | `0` = Ascending (default), `1` = Descending        |

Get all restaurants response:

```json
{
  "statusCode": 200,
  "message": "All restaurants have been successfully retrieved",
  "data": {
    "items": [
      {
        "id": "cdd15c8a-6cf2-43ea-bd02-285144cfc93d",
        "name": "KFC",
        "description": "American fast food chain specializing in fried chicken.",
        "category": "Fast Food",
        "hasDelivery": true,
        "city": "London",
        "street": "Cork St 5",
        "postalCode": "12-345",
        "dishes": []
      }
    ],
    "totalPages": 1,
    "totalItemsCount": 1,
    "itemsFrom": 1,
    "itemsTo": 10
  }
}
```

### Dishes

All dish endpoints are scoped under a restaurant: `/api/restaurants/{restaurantId}/Dish`.

| Method | Endpoint                                                  | Auth | Description                    |
| ------ | --------------------------------------------------------- | ---- | ------------------------------ |
| GET    | `/api/restaurants/{restaurantId}/Dish`                    | Auth | List dishes for a restaurant   |
| GET    | `/api/restaurants/{restaurantId}/Dish/{dishId}`           | Auth | Get a single dish              |
| POST   | `/api/restaurants/{restaurantId}/Dish`                    | Auth | Create a dish                  |
| PUT    | `/api/restaurants/{restaurantId}/Dish/{dishId}`           | Auth | Update a dish                  |
| DELETE | `/api/restaurants/{restaurantId}/Dish/{dishId}`           | Auth | Delete a single dish           |
| DELETE | `/api/restaurants/{restaurantId}/Dish`                    | Auth | Delete all dishes for a restaurant |

> `{restaurantId}` must match the dish's owning restaurant — create, update, and delete operations validate this.

Create / Update dish body:

```json
{
  "name": "Nashville Hot Chicken",
  "description": "Nashville Hot Chicken (10 pcs.)",
  "price": 10.30,
  "kiloCalories": 850
}
```

### Users

| Method | Endpoint                 | Auth                  | Description                        |
| ------ | ------------------------ | --------------------- | ---------------------------------- |
| PUT    | `/api/Identity/user`     | Authenticated user    | Update nationality / date of birth |
| POST   | `/api/Identity/userRole` | Role: `Administrator` | Assign a role to a user            |
| DELETE | `/api/Identity`          | Role: `Administrator` | Remove a role from a user          |

Update user details body:

```json
{
  "dateOfBirth": "1995-05-10",
  "nationality": "Ghanaian"
}
```

Assign / remove role body:

```json
{
  "userEmail": "jane@example.com",
  "roleName": "Owner"
}
```

## Response Envelope

Every endpoint returns a consistent envelope:

```json
{
  "statusCode": 200,
  "message": "Resturants retrieved successfully.",
  "data": { }
}
```

| Field        | Description                          |
| ------------ | ------------------------------------ |
| `statusCode` | HTTP status code                     |
| `message`    | Human-readable result message        |
| `data`       | Payload (object, array, or null)     |

### Common status codes

| Code | Meaning                                        |
| ---- | ---------------------------------------------- |
| 200  | OK                                             |
| 201  | Created (with Location header)                 |
| 204  | No Content (update / delete)                   |
| 400  | Invalid request or validation failure          |
| 401  | Missing or invalid authentication              |
| 403  | Authenticated but not authorized for the role  |
| 404  | Resource not found                             |
| 500  | Unexpected server error                        |

## Error Handling

- **Validation failures** are handled automatically by `[ApiController]` and return `400` with field-level errors.
- **Unhandled exceptions** are caught by a central `ErrorHandlingMiddleware` and returned as RFC 7807 `ProblemDetails`:

| Exception            | HTTP Status | Title                |
| -------------------- | ----------- | -------------------- |
| `NotFoundException`  | 404         | Not Found            |
| `ForbiddenException` | 403         | Forbidden            |
| any other `Exception`| 500         | Internal Server Error|

Each response includes the request's `TraceIdentifier`, which is also written to the logs (with the exception and stack trace) for correlation. Exception details are never exposed to the client.

## Migrations

Migrations live in `Restaurants.Infrastructure/Migrations`. To add a new migration:

```bash
dotnet ef migrations add <MigrationName> --project Restaurants.Infrastructure --startup-project Restaurants.API
```

## License

MIT License
