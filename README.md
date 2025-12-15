# CRM Exception Flow - ITI Graduation Project

## 📌 Project Overview
**CRM Exception Flow** is a comprehensive Customer Relationship Management (CRM) system designed to streamline business operations, manage customer interactions, and handle exception flows efficiently. This project is developed as part of the ITI Graduation Project.

The system incorporates **AI-powered recommendations** to assist users in making data-driven decisions and resolving exceptions effectively.

## 🚀 Tech Stack

### Backend
- **Framework**: .NET Core 8 / ASP.NET Core Web API
- **Language**: C#
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JWT (JSON Web Tokens)

### Frontend
- **Framework**: Angular
- **Styling**: SCSS / CSS
- **State Management**: RxJS

### Key Features
- **User Management**: Role-based access control (Admin, Manager, Employee, IT Support).
- **Customer & Lead Management**: Create, update, and track customer details and lifecycle.
- **Deal Management**: Track sales opportunities and deal stages.
- **Exception Flow Handling**: specialized workflow for managing and resolving business exceptions.
- **AI Recommendations**: Intelligent suggestions for resolving exceptions and optimizing workflows.
- **Dashboard & Analytics**: Visual insights into business performance and system usage.
- **Interaction Logging**: Keep a history of all customer interactions.

## 📂 Project Structure
```
ITI-Graduation-Project/
├── CRM_ExceptionFlow/          # Backend Solution
│   ├── CRM_ExceptionFlow/      # ASP.NET Core Web API Project
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── ...
│   └── ...
└── crm-exceptionflow-ui/       # Frontend Application (Angular)
    ├── src/
    ├── angular.json
    └── ...
```

## 🛠️ Setup & Installation

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or later recommended)
- [Node.js](https://nodejs.org/) (LTS version)
- [SQL Server](https://www.microsoft.com/sql-server/)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)

### Backend Setup
1. Navigate to the backend directory:
   ```bash
   cd CRM_ExceptionFlow/CRM_ExceptionFlow
   ```
2. Configure the database connection string in `appsettings.json`.
3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:7152` (or similar).

### Frontend Setup
1. Navigate to the frontend directory:
   ```bash
   cd crm-exceptionflow-ui
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   ng serve
   ```
   The application will be available at `http://localhost:4200`.

## 🤝 Contributing
1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m 'Add some feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a Pull Request.

## 📄 License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
