# Good Hamburger Project

## Table of Contents
- [Introduction](#introduction)
- [Architecture](#architecture)
- [Technologies and Frameworks Used](#technologies-and-frameworks-used)
- [Setup and Installation](#setup-and-installation)
  - [Prerequisites](#prerequisites)
  - [Building the Project](#building-the-project)
  - [Running with Docker Compose (PostgreSQL)](#running-with-docker-compose-postgresql)
  - [API Access (Swagger UI)](#api-access-swagger-ui)
- [Testing](#testing)
- [Known Limitations or Assumptions](#known-limitations-or-assumptions)
- [Possible Improvements](#possible-improvements)
- [Unit Test Project Coverage](#unit-test-project-coverage)

## Introduction
This project is a backend API for a "Good Hamburger" ordering system, built with ASP.NET Core. It demonstrates a layered architecture, incorporating best practices for domain-driven design, dependency injection, and data persistence.

## Architecture
The project follows a layered architecture, promoting separation of concerns and maintainability.

-   **GoodHamburger.API:** The entry point of the application, handling HTTP requests. It's an ASP.NET Core Web API project.
    -   **Responsibilities:** API controllers, request/response handling, middleware, dependency injection configuration.
    -   **Dependencies:** `GoodHamburger.Core`, `GoodHamburger.Infrastructure`.
-   **GoodHamburger.Core:** Contains application-specific business logic, interfaces for services and repositories, DTOs (Data Transfer Objects), and application-level validations.
    -   **Responsibilities:** Application services (e.g., `OrderService`, `MenuService`), DTO definitions, interfaces for domain operations.
    -   **Dependencies:** `GoodHamburger.Domain`.
-   **GoodHamburger.Domain:** The core of the application, defining business entities, value objects, and domain-specific exceptions. It represents the business rules and data.
    -   **Responsibilities:** `OrderEntity`, `SandwichEntity`, `ExtraEntity`, `OrderDetailEntity`, domain exceptions, business rules (e.g., discount calculation within `OrderEntity`).
    -   **Dependencies:** None (pure domain).
-   **GoodHamburger.Infrastructure:** Implements interfaces defined in `GoodHamburger.Core` for data access (repositories) and other infrastructure concerns.
    -   **Responsibilities:** Entity Framework Core `DbContext` (`AppDbContext`), concrete repository implementations (`OrderRepository`, `SandwichRepository`, `ExtraRepository`), database migrations.
    -   **Dependencies:** `GoodHamburger.Domain`, `GoodHamburger.Core`.
-   **GoodHamburger.Test:** Contains unit and integration tests for the other projects, ensuring the correctness and reliability of the application.
    -   **Responsibilities:** Test classes using xUnit, Moq for mocking dependencies.
    -   **Dependencies:** `GoodHamburger.Core`, `GoodHamburger.Domain`, `GoodHamburger.Infrastructure`.

## Technologies and Frameworks Used
-   **Backend:** ASP.NET Core 8.0
-   **Language:** C#
-   **Database:** PostgreSQL (Dockerized)
-   **ORM:** Entity Framework Core 8.0
-   **API Documentation:** Swagger/Swashbuckle
-   **Mapping:** AutoMapper
-   **Validation:** FluentValidation
-   **Testing:** xUnit, Moq
-   **Containerization:** Docker, Docker Compose

## Setup and Installation

### Prerequisites
-   .NET SDK 8.0 or later
-   Docker Desktop (or Docker Engine)

### Building the Project
Navigate to the root directory of the project and run:
```bash
dotnet build
```

### Running with Docker Compose (PostgreSQL)
This project is configured to run with a PostgreSQL database using Docker Compose.

1.  **Ensure Docker is running.**
2.  **Start the services:**
    ```bash
    docker-compose up --build -d
    ```
    This command will build the API Docker image, pull the PostgreSQL image, and start both services in detached mode.
3.  **Verify services are running:**
    ```bash
    docker-compose ps
    ```
4.  **Apply Database Migrations (on application startup):**
    The application is configured to automatically apply pending Entity Framework Core migrations on startup.
    Ensure that no previous conflicting database exists on `localhost:5500` if you run into issues. If you do, manually drop the `goodhamburger` database from your local PostgreSQL instance (e.g., via PgAdmin).

### API Access (Swagger UI)
Once the `good-hamburger-api` container is running, the API documentation (Swagger UI) will be available at:
[http://localhost:5000/swagger](http://localhost:5000/swagger)

You can use this interface to explore the API endpoints and send requests.

## Testing
The `GoodHamburger.Test` project contains unit tests for the application's components.

### Running Tests
Navigate to the `GoodHamburger.Test` directory and run:
```bash
dotnet test
```

## Known Limitations or Assumptions
-   **Authentication/Authorization:** No authentication or authorization mechanisms are implemented.
-   **Error Handling:** Generic `DomainException` is used for most business rule violations. More granular custom exceptions could provide better error detail.
-   **"Fries" and "Soft drink" Validation:** Specific validations for "Only one serving of Fries allowed" and "Only one serving of Soft drink allowed" are handled in the `OrderService` (application layer) due to their dependency on `ExtraEntity` names, which are not typically accessible in a DTO validator without repository injection. A more sophisticated domain model might integrate this logic directly into a Value Object or Domain Service.
-   **HTTPS Redirection:** A warning regarding HTTPS redirection might appear in logs during development, as HTTPS is not fully configured for the Docker environment in this setup.

## Possible Improvements
-   **Richer Domain Model:** Further encapsulate business rules within domain entities (e.g., `OrderEntity` could handle more complex state transitions or validations).
-   **More Granular Exception Handling:** Introduce more specific custom exceptions for different types of business errors to provide clearer feedback to clients.
-   **Centralized Logging:** Implement a structured logging solution (e.g., Serilog) for better observability.
-   **Authentication and Authorization:** Implement industry-standard security measures (e.g., JWT authentication).
-   **API Versioning:** Introduce API versioning for better manageability of API evolution.
-   **Database Seeding:** For production-like environments, consider a more robust data seeding strategy (e.g., using a dedicated tool or a more controlled migration approach).
-   **Automated Testing:** Expand unit and integration test coverage.
-   **Performance Optimization:** Implement caching mechanisms, optimize database queries, or use asynchronous programming more extensively where beneficial.

## Unit Test Project Coverage
The `GoodHamburger.Test` project currently includes basic unit tests for the `OrderService`, specifically covering:
-   Discount calculation logic for different combinations of sandwich and extras.
-   Handling of duplicate extras.
This coverage can and should be expanded to cover all critical business logic and edge cases in all layers of the application.
