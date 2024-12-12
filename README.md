# MiniCommerce

MiniCommerce is a modern e-commerce solution built with .NET using Clean Architecture principles. This project implements a scalable and maintainable e-commerce platform with a focus on clean code and best practices.

## Project Structure

The solution follows Clean Architecture and is organized into the following projects:

- **MiniCommerce.Api**: Main API project that handles HTTP requests and serves as the entry point of the application
- **MiniCommerce.Application**: Contains application business logic, use cases, and DTOs
- **MiniCommerce.Domain**: Core domain entities, interfaces, and business rules
- **MiniCommerce.Common**: Shared utilities and common functionality
- **MiniCommerce.Infra**: Infrastructure implementation
- **MiniCommerce.Infrastructure**: Additional infrastructure concerns

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Docker Support
- Clean Architecture
- CQRS Pattern

## Prerequisites

- .NET 8 SDK 
- Docker (Optional, for containerization)
- PostgreSQL 16

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/yourusername/MiniCommerce.git
```

2. Navigate to the solution directory:
```bash
cd MiniCommerce
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Update the database connection string in `appsettings.json` or use user secrets

5. Run the migrations:
```bash
dotnet ef database update
```

6. Run the application:
```bash
dotnet run --project Src/MiniCommerce.Api
```

## Docker Support

The project includes Docker support. To run using Docker:

```bash
docker-compose up -d
```

## Architecture

The solution follows Clean Architecture principles with the following layers:

- **API Layer**: REST API endpoints and controllers
- **Application Layer**: Application business logic and use cases
- **Domain Layer**: Core business logic and domain entities
- **Infrastructure Layer**: External concerns implementation (database, external services)

## Features

- Clean Architecture implementation
- CQRS pattern
- Domain-Driven Design principles
- API endpoints for e-commerce operations
- Containerization support
- Scalable and maintainable codebase

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
