Assignment
A backend API project designed for handling OTP (One-Time Password) generation and verification via SMS and Email. The application utilizes Redis for temporary OTP storage and logs communications using Entity Framework Core with SQL Server.

Features
OTP Generation: Create secure OTPs for both SMS and Email channels.

OTP Verification: Validate user-submitted OTPs against stored entries.

Redis Integration: Store OTPs temporarily with expiration handling.

Logging: Record SMS and Email communications in the database.

API Documentation: Interactive Swagger UI for testing and documentation.

Modular Architecture: Organized into Application, Domain, and Infrastructure layers.

Docker Support: Containerized setup for easy deployment.

Technologies Used
ASP.NET Core

Entity Framework Core

Redis

FluentValidation

Docker & Docker Compose

Swagger (Swashbuckle)

Getting Started
Prerequisites
.NET 7 SDK

Docker

Redis

SQL Server

git clone https://github.com/developertarik2/Assignment.git
cd Assignment

Assignment/
├── Assignment.Application/       # Application logic and services
├── Assignment.Domain/            # Domain models and interfaces
├── Assignment.Infrastructure/    # Data access and external services
├── Assignment/                   # API project (controllers, startup)
├── docker-compose.yml            # Docker Compose configuration
├── appsettings.json              # Application configuration
└── README.md                     # Project documentation
