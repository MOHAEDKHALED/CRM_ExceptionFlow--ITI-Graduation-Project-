# CRM Exception Flow System

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-20.3-DD0031?logo=angular)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A comprehensive Exception Management System for CRM platforms built with **Clean Architecture** and **Domain-Driven Design (DDD)**, featuring a modern user interface and intelligent AI-powered recommendations.

---

## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Authentication](#authentication)
- [API Documentation](#api-documentation)
- [AI Integration](#ai-integration)
- [Testing](#testing)
- [Configuration](#configuration)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

CRM Exception Flow System is an enterprise-grade solution designed to manage and track exceptions in CRM software projects. The system implements industry best practices including Clean Architecture, Domain-Driven Design, and provides seamless integration with AI services for intelligent exception resolution recommendations.

---

## Key Features

### Exception Management
- Track and monitor exceptions across multiple software projects
- Comprehensive exception lifecycle management
- Real-time status updates and notifications
- Advanced filtering and search capabilities

### AI-Powered Recommendations
- Integration with N8N workflow automation
- Intelligent exception resolution suggestions
- Machine learning-based pattern recognition
- Automated recommendation storage and retrieval

### User Management
- Advanced role-based access control (RBAC)
- Four distinct user roles: Admin, Manager, Employee, IT Support
- Secure user authentication and authorization
- User activity tracking and audit logs

### Customer Relationship Management
- Complete customer profile management
- Deal tracking and pipeline management
- Interaction history and communication logs
- Customer analytics and insights

### Dashboard and Analytics
- Comprehensive statistics and KPIs
- Real-time performance indicators
- Customizable reports and visualizations
- Export capabilities for data analysis

### Security
- JWT-based authentication system
- BCrypt password hashing
- Secure API endpoints with role-based authorization
- CORS protection and security headers

### Modern User Interface
- Responsive design for all devices
- Smooth animations and transitions
- Intuitive navigation and user experience
- Dark mode support

---

## Architecture

The project follows **Clean Architecture** principles combined with **Domain-Driven Design (DDD)** patterns, ensuring maintainability, testability, and scalability.

### Architecture Layers

```
backend/
├── Domain/              # Core Business Logic Layer
│   ├── Entities/        # Business entities
│   ├── ValueObjects/    # Immutable value objects
│   ├── Repositories/    # Repository contracts
│   └── Common/          # Base classes and domain events
│
├── Application/         # Application Business Rules Layer
│   ├── UseCases/        # Application use cases
│   ├── DTOs/            # Data transfer objects
│   ├── Interfaces/      # Application interfaces
│   └── Common/          # Mappings and utilities
│
├── Infrastructure/      # External Concerns Layer
│   ├── Data/            # Database context and migrations
│   ├── Repositories/    # Repository implementations
│   ├── Services/        # External service integrations
│   └── Middleware/      # Custom middleware components
│
└── Presentation/        # User Interface Layer
    └── API/             # REST API controllers
        ├── Controllers/ # API endpoints
        └── Program.cs   # Application entry point
```

### Design Principles

- **Separation of Concerns**: Each layer has distinct responsibilities
- **Dependency Inversion**: Dependencies point inward toward the domain
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed Principle**: Open for extension, closed for modification
- **Interface Segregation**: Clients depend only on interfaces they use
- **DRY (Don't Repeat Yourself)**: Code reusability and maintainability

---

## Technology Stack

### Backend Technologies

- **.NET 9.0**: Latest .NET framework with improved performance
- **ASP.NET Core Web API**: RESTful API development
- **Entity Framework Core 9.0**: Object-Relational Mapping (ORM)
- **SQL Server 2022**: Enterprise database management
- **JWT Bearer Authentication**: Stateless authentication mechanism
- **BCrypt.Net**: Secure password hashing algorithm
- **AutoMapper**: Object-to-object mapping
- **Swagger/OpenAPI**: Interactive API documentation
- **LINQ**: Language Integrated Query for data manipulation

### Frontend Technologies

- **Angular 20.3**: Latest Angular framework
- **TypeScript 5.9**: Strongly-typed JavaScript superset
- **RxJS**: Reactive programming with observables
- **Standalone Components**: Modern Angular architecture
- **Angular Animations**: Fluid UI transitions
- **SCSS**: Advanced CSS preprocessing
- **Angular Router**: Client-side navigation
- **HttpClient**: HTTP communication with backend

### Development Tools

- **Visual Studio 2022**: Integrated development environment
- **Visual Studio Code**: Lightweight code editor
- **SQL Server Management Studio (SSMS)**: Database management
- **Postman**: API testing and development
- **Git**: Version control system

---

## Prerequisites

### Backend Requirements

- **Visual Studio 2022** or later
- **.NET 9.0 SDK** ([Download](https://dotnet.microsoft.com/download))
- **SQL Server 2019** or later
- **SQL Server Management Studio (SSMS)**
- **Git** for version control

### Frontend Requirements

- **Node.js 18.x** or later ([Download](https://nodejs.org/))
- **npm** (comes with Node.js) or **yarn**
- **Angular CLI**: Install globally via `npm install -g @angular/cli`

### Optional Tools

- **Postman** for API testing
- **Docker** for containerization
- **Redis** for caching (optional)

---

## Installation

### Step 1: Clone the Repository

```bash
git clone https://github.com/yourusername/CRM_ExceptionFlow.git
cd CRM_ExceptionFlow
```

### Step 2: Backend Setup

#### 2.1 Navigate to Backend Directory

```bash
cd backend
```

#### 2.2 Configure Application Settings

Open `appsettings.json` and update the following configurations:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CRM_ExceptionFlow;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_AT_LEAST_64_CHARACTERS_LONG_FOR_SECURITY",
    "Issuer": "CRM_ExceptionFlow",
    "Audience": "CRM_Users",
    "ExpiryMinutes": 120
  },
  "N8N": {
    "WebhookUrl": "YOUR_N8N_WEBHOOK_URL"
  }
}
```

**Important Security Notes:**
- Replace `YOUR_SERVER` with your SQL Server instance name
- Generate a strong secret key for JWT (minimum 64 characters)
- Update N8N webhook URL if using AI recommendations

#### 2.3 Restore NuGet Packages

Open `CRM.sln` in Visual Studio 2022, then:

```powershell
# In Package Manager Console
dotnet restore
```

Or restore packages in Visual Studio by right-clicking the solution and selecting "Restore NuGet Packages".

#### 2.4 Create Database Migrations

```powershell
# In Package Manager Console (Visual Studio)
Add-Migration InitialCreate -Project CRM.Infrastructure -StartupProject CRM.Presentation.API

# Apply migration to database
Update-Database -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
```

Alternative using .NET CLI:

```bash
# Navigate to API project directory
cd src/Presentation/API

# Create migration
dotnet ef migrations add InitialCreate --project ../../../Infrastructure/CRM.Infrastructure.csproj

# Update database
dotnet ef database update --project ../../../Infrastructure/CRM.Infrastructure.csproj
```

#### 2.5 Run the Backend

**Option 1: Using Visual Studio**
- Press `F5` or click the "Run" button
- Backend will start on: `http://localhost:5000`
- Swagger UI available at: `http://localhost:5000/swagger`

**Option 2: Using .NET CLI**

```bash
cd src/Presentation/API
dotnet run
```

### Step 3: Frontend Setup

#### 3.1 Navigate to Frontend Directory

```bash
cd frontend
```

#### 3.2 Install Dependencies

```bash
npm install
```

Or using yarn:

```bash
yarn install
```

#### 3.3 Configure API Endpoint

Update `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'
};
```

#### 3.4 Run the Frontend

```bash
npm start
```

Or using Angular CLI:

```bash
ng serve
```

The application will be available at: `http://localhost:4200`

### Step 4: Initial Login

Use the default administrator credentials:

- **Username**: `admin`
- **Password**: `Admin@123`

**Security Warning**: Change the default password immediately after first login in production environments.

---

## Project Structure

### Backend Structure

```
backend/
├── src/
│   ├── Domain/                          # Core Business Logic
│   │   ├── Entities/                    # Domain entities
│   │   │   ├── User.cs
│   │   │   ├── Exception.cs
│   │   │   ├── Customer.cs
│   │   │   └── Deal.cs
│   │   ├── ValueObjects/                # Immutable value objects
│   │   │   ├── Email.cs
│   │   │   └── PhoneNumber.cs
│   │   ├── Repositories/                # Repository interfaces
│   │   │   ├── IUserRepository.cs
│   │   │   └── IExceptionRepository.cs
│   │   └── Common/                      # Base classes
│   │       ├── BaseEntity.cs
│   │       └── DomainEvent.cs
│   │
│   ├── Application/                     # Application Logic
│   │   ├── UseCases/                    # Business use cases
│   │   │   ├── Exceptions/
│   │   │   ├── Users/
│   │   │   └── Customers/
│   │   ├── DTOs/                        # Data transfer objects
│   │   │   ├── ExceptionDto.cs
│   │   │   └── UserDto.cs
│   │   ├── Interfaces/                  # Service interfaces
│   │   │   ├── IAuthService.cs
│   │   │   └── IN8NService.cs
│   │   └── Common/                      # Mappings
│   │       └── MappingProfile.cs
│   │
│   ├── Infrastructure/                  # Infrastructure Concerns
│   │   ├── Data/                        # Database context
│   │   │   ├── ApplicationDbContext.cs
│   │   │   └── Migrations/
│   │   ├── Repositories/                # Repository implementations
│   │   │   ├── UserRepository.cs
│   │   │   └── ExceptionRepository.cs
│   │   ├── Services/                    # External services
│   │   │   ├── AuthService.cs
│   │   │   └── N8NService.cs
│   │   └── Middleware/                  # Custom middleware
│   │       └── ExceptionHandlingMiddleware.cs
│   │
│   └── Presentation/                    # API Layer
│       └── API/
│           ├── Controllers/             # API controllers
│           │   ├── AuthController.cs
│           │   ├── ExceptionsController.cs
│           │   ├── UsersController.cs
│           │   └── CustomersController.cs
│           ├── Program.cs               # Application startup
│           └── appsettings.json         # Configuration
│
├── tests/                               # Unit and integration tests
│   ├── Domain.Tests/
│   ├── Application.Tests/
│   └── API.Tests/
│
└── CRM.sln                              # Solution file
```

### Frontend Structure

```
frontend/
├── src/
│   └── app/
│       ├── core/                        # Core module
│       │   ├── services/                # Core services
│       │   │   ├── auth.service.ts
│       │   │   └── api.service.ts
│       │   ├── guards/                  # Route guards
│       │   │   └── auth.guard.ts
│       │   ├── interceptors/            # HTTP interceptors
│       │   │   └── auth.interceptor.ts
│       │   └── models/                  # TypeScript models
│       │
│       ├── features/                    # Feature modules
│       │   ├── auth/                    # Authentication
│       │   │   ├── login/
│       │   │   └── register/
│       │   ├── dashboard/               # Dashboard
│       │   ├── exceptions/              # Exception management
│       │   │   ├── exception-list/
│       │   │   ├── exception-detail/
│       │   │   └── exception-form/
│       │   ├── users/                   # User management
│       │   └── customers/               # Customer management
│       │
│       ├── shared/                      # Shared module
│       │   ├── components/              # Reusable components
│       │   │   ├── header/
│       │   │   ├── sidebar/
│       │   │   └── footer/
│       │   ├── directives/              # Custom directives
│       │   └── pipes/                   # Custom pipes
│       │
│       ├── app.component.ts             # Root component
│       ├── app.routes.ts                # Application routes
│       └── app.config.ts                # Application configuration
│
├── src/environments/                    # Environment configurations
│   ├── environment.ts
│   └── environment.prod.ts
│
├── angular.json                         # Angular configuration
├── package.json                         # NPM dependencies
└── tsconfig.json                        # TypeScript configuration
```

---

## Authentication

The system implements JWT (JSON Web Token) based authentication with the following specifications:

### Authentication Flow

1. **User Login**: Client sends credentials to `/api/auth/login`
2. **Token Generation**: Server validates credentials and generates JWT
3. **Token Storage**: Client stores token (typically in localStorage)
4. **Authenticated Requests**: Client includes token in Authorization header
5. **Token Validation**: Server validates token on each request
6. **Token Refresh**: Token expires after 120 minutes (configurable)

### Security Features

- **Password Hashing**: BCrypt algorithm with salt rounds
- **Token Expiration**: Configurable token lifetime
- **Role-Based Authorization**: Four distinct user roles
- **Secure Headers**: HTTPS enforcement in production
- **CORS Protection**: Configured allowed origins

### User Roles and Permissions

#### Admin
- Full system access
- User management (create, update, delete)
- System configuration
- All CRUD operations

#### Manager
- Exception management
- Customer management
- Team oversight
- Report generation

#### Employee
- View and create exceptions
- Update assigned exceptions
- View customer information
- Limited reporting

#### IT Support
- Technical exception handling
- System monitoring
- Log access
- Technical reports

### JWT Token Structure

```json
{
  "sub": "user_id",
  "username": "admin",
  "role": "Admin",
  "email": "admin@example.com",
  "exp": 1735401600,
  "iss": "CRM_ExceptionFlow",
  "aud": "CRM_Users"
}
```

---

## API Documentation

### Authentication Endpoints

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}

Response: 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "role": "Admin",
  "expiresAt": "2025-12-28T10:00:00Z"
}
```

#### Register
```http
POST /api/auth/register
Content-Type: application/json
Authorization: Bearer {admin_token}

{
  "username": "newuser",
  "email": "user@example.com",
  "password": "SecurePass@123",
  "fullName": "John Doe",
  "role": "Employee"
}

Response: 201 Created
{
  "id": "guid",
  "username": "newuser",
  "email": "user@example.com",
  "role": "Employee"
}
```

#### Get Profile
```http
GET /api/auth/profile
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "username": "admin",
  "email": "admin@example.com",
  "fullName": "System Administrator",
  "role": "Admin"
}
```

### Exception Management Endpoints

#### List Exceptions
```http
GET /api/exceptions?page=1&pageSize=10&status=Open
Authorization: Bearer {token}

Response: 200 OK
{
  "data": [
    {
      "id": "guid",
      "title": "Null Reference Exception",
      "description": "Object reference not set to an instance",
      "status": "Open",
      "priority": "High",
      "createdAt": "2025-12-28T08:00:00Z",
      "assignedTo": "John Doe"
    }
  ],
  "totalCount": 50,
  "page": 1,
  "pageSize": 10
}
```

#### Create Exception
```http
POST /api/exceptions
Content-Type: application/json
Authorization: Bearer {token}

{
  "title": "Database Connection Error",
  "description": "Unable to connect to SQL Server",
  "status": "Open",
  "priority": "Critical",
  "projectId": "guid",
  "assignedToId": "guid"
}

Response: 201 Created
{
  "id": "guid",
  "title": "Database Connection Error",
  "aiRecommendation": "Check connection string configuration..."
}
```

#### Get Exception Details
```http
GET /api/exceptions/{id}
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "title": "Database Connection Error",
  "description": "Unable to connect to SQL Server",
  "status": "Open",
  "priority": "Critical",
  "createdAt": "2025-12-28T08:00:00Z",
  "updatedAt": "2025-12-28T09:00:00Z",
  "aiRecommendation": "Check connection string...",
  "comments": [],
  "attachments": []
}
```

#### Update Exception
```http
PUT /api/exceptions/{id}
Content-Type: application/json
Authorization: Bearer {token}

{
  "status": "InProgress",
  "assignedToId": "guid",
  "notes": "Working on the connection string issue"
}

Response: 200 OK
```

#### Delete Exception
```http
DELETE /api/exceptions/{id}
Authorization: Bearer {token}

Response: 204 No Content
```

### User Management Endpoints

#### List Users
```http
GET /api/users?page=1&pageSize=20
Authorization: Bearer {token}

Response: 200 OK
{
  "data": [
    {
      "id": "guid",
      "username": "user1",
      "email": "user1@example.com",
      "fullName": "User One",
      "role": "Employee",
      "isActive": true
    }
  ],
  "totalCount": 100
}
```

#### Get User Details
```http
GET /api/users/{id}
Authorization: Bearer {token}

Response: 200 OK
{
  "id": "guid",
  "username": "user1",
  "email": "user1@example.com",
  "fullName": "User One",
  "role": "Employee",
  "createdAt": "2025-01-01T00:00:00Z",
  "lastLogin": "2025-12-28T08:00:00Z"
}
```

### Customer Management Endpoints

#### List Customers
```http
GET /api/customers?page=1&pageSize=10
Authorization: Bearer {token}

Response: 200 OK
{
  "data": [
    {
      "id": "guid",
      "companyName": "Acme Corp",
      "contactPerson": "Jane Smith",
      "email": "jane@acme.com",
      "phone": "+1234567890",
      "status": "Active"
    }
  ]
}
```

#### Create Customer
```http
POST /api/customers
Content-Type: application/json
Authorization: Bearer {token}

{
  "companyName": "New Company Ltd",
  "contactPerson": "Bob Johnson",
  "email": "bob@newcompany.com",
  "phone": "+1987654321",
  "address": "123 Business St"
}

Response: 201 Created
```

### Dashboard Endpoints

#### Get Dashboard Statistics
```http
GET /api/dashboard
Authorization: Bearer {token}

Response: 200 OK
{
  "totalExceptions": 150,
  "openExceptions": 45,
  "inProgressExceptions": 30,
  "resolvedExceptions": 75,
  "totalCustomers": 50,
  "activeDeals": 25,
  "monthlyRevenue": 150000,
  "recentActivity": []
}
```

---

## AI Integration

The system integrates with N8N workflow automation platform to provide intelligent exception resolution recommendations.

### How It Works

1. **Exception Creation**: When a new exception is created, the system automatically triggers an AI analysis
2. **N8N Webhook**: Exception details are sent to the configured N8N webhook endpoint
3. **AI Processing**: N8N workflow processes the exception using AI/ML models
4. **Recommendation Generation**: AI generates contextual recommendations based on:
   - Exception type and severity
   - Historical resolution patterns
   - Similar past exceptions
   - Best practices database
5. **Storage**: Recommendations are stored in the database and linked to the exception
6. **Display**: Users can view AI recommendations in the exception details page

### Configuration

Update `appsettings.json` with your N8N webhook URL:

```json
{
  "N8N": {
    "WebhookUrl": "https://your-n8n-instance.com/webhook/exception-analysis",
    "Timeout": 30,
    "RetryAttempts": 3
  }
}
```

### Request Payload

```json
{
  "exceptionId": "guid",
  "title": "Database Connection Error",
  "description": "Unable to connect to SQL Server instance",
  "stackTrace": "at System.Data.SqlClient...",
  "priority": "Critical",
  "projectName": "CRM System",
  "environment": "Production"
}
```

### Response Format

```json
{
  "recommendation": "Check the following...",
  "confidence": 0.95,
  "suggestedActions": [
    "Verify SQL Server service is running",
    "Check connection string configuration",
    "Validate network connectivity"
  ],
  "relatedDocuments": [
    "https://docs.example.com/troubleshooting"
  ]
}
```

---

## Testing

### Backend Testing

#### Using Swagger UI

1. Navigate to `http://localhost:5000/swagger`
2. Authenticate:
   - Click "Authorize" button
   - Login using POST `/api/auth/login`
   - Copy the returned token
   - Enter: `Bearer {your-token}`
3. Test endpoints by expanding operations and clicking "Try it out"

#### Using Postman

1. Import the Swagger/OpenAPI specification
2. Create an environment with:
   - `baseUrl`: `http://localhost:5000`
   - `token`: `{obtained-from-login}`
3. Test endpoints using the collection

#### Unit Tests

```bash
cd backend
dotnet test
```

### Frontend Testing

#### Unit Tests

```bash
cd frontend
npm test
```

#### End-to-End Tests

```bash
npm run e2e
```

#### Manual Testing Checklist

- [ ] Login with valid credentials
- [ ] Login with invalid credentials (should fail)
- [ ] Create new exception
- [ ] View exception details
- [ ] Update exception status
- [ ] Delete exception
- [ ] Create new user (Admin only)
- [ ] View dashboard statistics
- [ ] Test role-based access control
- [ ] Test responsive design on mobile
- [ ] Test all CRUD operations

---

## Configuration

### Backend Configuration

#### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CRM_ExceptionFlow;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-super-secret-key-minimum-64-characters-for-production-security",
    "Issuer": "CRM_ExceptionFlow",
    "Audience": "CRM_Users",
    "ExpiryMinutes": 120
  },
  "N8N": {
    "WebhookUrl": "https://your-n8n-instance.com/webhook/exception-analysis",
    "Timeout": 30,
    "RetryAttempts": 3
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "https://your-production-domain.com"
    ]
  }
}
```

### Frontend Configuration

#### environment.ts (Development)

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  tokenKey: 'crm_auth_token',
  refreshTokenKey: 'crm_refresh_token'
};
```

#### environment.prod.ts (Production)

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.your-domain.com/api',
  tokenKey: 'crm_auth_token',
  refreshTokenKey: 'crm_refresh_token'
};
```

---

## Troubleshooting

### Common Backend Issues

#### Issue: Cannot open database "CRM_ExceptionFlow"

**Solution:**
1. Verify SQL Server is running:
   ```bash
   # Check SQL Server service status
   Get-Service -Name MSSQLSERVER
   ```
2. Verify connection string in `appsettings.json`
3. Ensure migrations are applied:
   ```powershell
   Update-Database -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
   ```
4. Check SQL Server authentication mode (Windows Auth vs SQL Auth)

#### Issue: JWT key is missing or invalid

**Solution:**
1. Open `appsettings.json`
2. Ensure `Jwt:Key` is at least 64 characters long
3. Generate a strong key:
   ```bash
   # PowerShell
   -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 64 | % {[char]$_})
   ```

#### Issue: Migrations fail to apply

**Solution:**
1. Delete existing migrations folder
2. Drop the database
3. Create fresh migration:
   ```powershell
   Add-Migration InitialCreate -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
   Update-Database
   ```

#### Issue: Port 5000 already in use

**Solution:**
1. Change port in `launchSettings.json`:
   ```json
   "applicationUrl": "http://localhost:5001"
   ```
2. Update frontend environment.ts accordingly

### Common Frontend Issues

#### Issue: CORS error when calling API

**Solution:**
1. Verify backend CORS configuration in `Program.cs`:
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowAngular",
           builder => builder
               .WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader());
   });
   ```
2. Ensure frontend is running on configured port

#### Issue: Module not found errors

**Solution:**
```bash
# Clear node_modules and reinstall
rm -rf node_modules package-lock.json
npm install
```

#### Issue: Authentication token not persisting

**Solution:**
1. Check browser localStorage
2. Verify token is being saved in AuthService
3. Check interceptor is adding token to requests

#### Issue: Angular compilation errors

**Solution:**
```bash
# Clear Angular cache
npm run ng cache clean

# Rebuild
npm run build
```

### Database Issues

#### Reset Database

```sql
-- Drop database
USE master;
DROP DATABASE IF EXISTS CRM_ExceptionFlow;

-- Recreate and run migrations
CREATE DATABASE CRM_ExceptionFlow;
```

Then run migrations again.

#### View Applied Migrations

```sql
USE CRM_ExceptionFlow;
SELECT * FROM __EFMigrationsHistory;
```

---

## Contributing

We welcome contributions to improve the CRM Exception Flow System. Please follow these guidelines:

### Development Workflow

1. **Fork the Repository**
   ```bash
   git clone https://github.com/yourusername/CRM_ExceptionFlow.git
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make Your Changes**
   - Write clean, maintainable code
   - Follow existing code style and conventions
   - Add comments for complex logic
   - Update documentation if needed

4. **Test Your Changes**
   ```bash
   # Backend tests
   dotnet test

   # Frontend tests
   npm test
   ```

5. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "Add: Brief description of your changes"
   ```

   Commit message format:
   - `Add:` for new features
   - `Fix:` for bug fixes
   - `Update:` for modifications
   - `Remove:` for deletions
   - `Refactor:` for code improvements

6. **Push to Your Fork**
   ```bash
   git push origin feature/your-feature-name
   ```

7. **Create Pull Request**
   - Go to the original repository
   - Click "New Pull Request"
   - Provide clear description of changes
   - Reference any related issues

### Code Style Guidelines

#### C# Backend
- Follow Microsoft C# coding conventions
- Use meaningful variable and method names
- Keep methods focused and single-purpose
- Add XML documentation for public APIs
- Use async/await for I/O operations

#### TypeScript Frontend
- Follow Angular style guide
- Use TypeScript strict mode
- Implement interfaces for complex objects
- Use RxJS operators appropriately
- Keep components focused and reusable

### Pull Request Checklist

- [ ] Code follows project style guidelines
- [ ] All tests pass successfully
- [ ] New tests added for new features
- [ ] Documentation updated
- [ ] No console errors or warnings
- [ ] Commit messages are clear and descriptive
- [ ] Branch is up to date with main

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for
