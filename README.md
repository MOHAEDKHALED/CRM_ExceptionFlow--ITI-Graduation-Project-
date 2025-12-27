# 🚀 CRM Exception Flow System

<div dir="rtl">
</div>

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-20.3-DD0031?logo=angular)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

<div dir="rtl">

نظام متكامل لإدارة الاستثناءات (Exceptions) في أنظمة CRM باستخدام **Clean Architecture** و **Domain-Driven Design (DDD)** مع واجهة مستخدم حديثة وتوصيات ذكية من AI.

</div>

A comprehensive Exception Management System for CRM platforms built with **Clean Architecture** and **Domain-Driven Design (DDD)**, featuring a modern UI and AI-powered recommendations.

---

## ✨ Features / المميزات

<div dir="rtl">

### 🎯 المميزات الرئيسية

- ✅ **إدارة الاستثناءات**: تتبع ومعالجة الاستثناءات في المشاريع البرمجية
- ✅ **توصيات ذكية**: تكامل مع N8N للحصول على توصيات AI لمعالجة الاستثناءات
- ✅ **إدارة المستخدمين**: نظام صلاحيات متقدم (Admin, Manager, Employee, ITSupport)
- ✅ **إدارة العملاء**: تتبع العملاء والصفقات والتفاعلات
- ✅ **لوحة تحكم**: إحصائيات شاملة ومؤشرات أداء
- ✅ **مصادقة آمنة**: JWT Authentication مع BCrypt password hashing
- ✅ **تصميم حديث**: واجهة مستخدم جميلة مع animations وresponsive design

</div>

### 🎯 Key Features

- ✅ **Exception Management**: Track and handle exceptions in software projects
- ✅ **AI Recommendations**: Integration with N8N for AI-powered exception resolution recommendations
- ✅ **User Management**: Advanced role-based access control (Admin, Manager, Employee, ITSupport)
- ✅ **Customer Management**: Track customers, deals, and interactions
- ✅ **Dashboard**: Comprehensive statistics and performance indicators
- ✅ **Secure Authentication**: JWT Authentication with BCrypt password hashing
- ✅ **Modern Design**: Beautiful UI with animations and responsive design

---

## 🏗️ Architecture / البنية المعمارية

<div dir="rtl">

### Clean Architecture + DDD

المشروع مبني باستخدام **Clean Architecture** و **Domain-Driven Design** مع فصل واضح للطبقات:

```
backend/
├── Domain/          # الطبقة الأساسية - Entities, Value Objects, Domain Events
├── Application/     # Use Cases, DTOs, Interfaces
├── Infrastructure/ # EF Core, Repositories, External Services
└── Presentation/    # API Controllers, Middleware
```

</div>

### Clean Architecture + DDD

The project is built using **Clean Architecture** and **Domain-Driven Design** with clear layer separation:

```
backend/
├── Domain/          # Core layer - Entities, Value Objects, Domain Events
├── Application/     # Use Cases, DTOs, Interfaces
├── Infrastructure/  # EF Core, Repositories, External Services
└── Presentation/    # API Controllers, Middleware
```

---

## 🛠️ Tech Stack / التقنيات المستخدمة

### Backend
- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9.0** - ORM
- **SQL Server** - Database
- **JWT Bearer Authentication** - Secure authentication
- **BCrypt.Net** - Password hashing
- **AutoMapper** - Object mapping
- **Swagger/OpenAPI** - API documentation

### Frontend
- **Angular 20.3** - Latest Angular framework
- **TypeScript 5.9** - Type-safe JavaScript
- **RxJS** - Reactive programming
- **Standalone Components** - Modern Angular architecture
- **Angular Animations** - Smooth UI transitions
- **SCSS** - Advanced styling

---

## 📋 Prerequisites / المتطلبات

<div dir="rtl">

### للباك إند:
- Visual Studio 2022 أو أحدث
- .NET 9.0 SDK
- SQL Server 2019 أو أحدث
- SQL Server Management Studio (SSMS)

### للفرونت إند:
- Node.js 18.x أو أحدث
- npm أو yarn

</div>

### For Backend:
- Visual Studio 2022 or later
- .NET 9.0 SDK
- SQL Server 2019 or later
- SQL Server Management Studio (SSMS)

### For Frontend:
- Node.js 18.x or later
- npm or yarn

---

## 🚀 Getting Started / البدء السريع

<div dir="rtl">

### 1. استنساخ المشروع

```bash
git clone https://github.com/yourusername/CRM_ExceptionFlow.git
cd CRM_ExceptionFlow
```

### 2. إعداد الباك إند

```bash
cd backend
```

1. افتح `backend/CRM.sln` في Visual Studio
2. عدل `appsettings.json`:
   - حدّث `ConnectionString` حسب إعدادات SQL Server لديك
   - حدّث `Jwt:Key` بمفتاح سري قوي (64 حرف على الأقل)

3. أنشئ Migration:
   ```powershell
   # في Package Manager Console
   Add-Migration InitialCreate -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
   Update-Database -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
   ```

4. شغّل المشروع:
   - اضغط `F5` في Visual Studio
   - الباك إند سيعمل على: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`

### 3. إعداد الفرونت إند

```bash
cd frontend
npm install
npm start
```

الفرونت إند سيعمل على: `http://localhost:4200`

### 4. تسجيل الدخول

- **Username**: `admin`
- **Password**: `Admin@123`

</div>

### 1. Clone the repository

```bash
git clone https://github.com/yourusername/CRM_ExceptionFlow.git
cd CRM_ExceptionFlow
```

### 2. Setup Backend

```bash
cd backend
```

1. Open `backend/CRM.sln` in Visual Studio
2. Update `appsettings.json`:
   - Update `ConnectionString` according to your SQL Server settings
   - Update `Jwt:Key` with a strong secret key (at least 64 characters)

3. Create Migration:
   ```powershell
   # In Package Manager Console
   Add-Migration InitialCreate -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
   Update-Database -Project CRM.Infrastructure -StartupProject CRM.Presentation.API
   ```

4. Run the project:
   - Press `F5` in Visual Studio
   - Backend will run on: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`

### 3. Setup Frontend

```bash
cd frontend
npm install
npm start
```

Frontend will run on: `http://localhost:4200`

### 4. Login

- **Username**: `admin`
- **Password**: `Admin@123`

---

## 📁 Project Structure / هيكل المشروع

```
CRM_ExceptionFlow/
├── backend/
│   ├── src/
│   │   ├── Domain/              # Domain Layer
│   │   │   ├── Entities/        # Domain entities
│   │   │   ├── ValueObjects/    # Value objects
│   │   │   ├── Repositories/    # Repository interfaces
│   │   │   └── Common/           # Base classes, events
│   │   ├── Application/         # Application Layer
│   │   │   ├── UseCases/        # Business logic
│   │   │   ├── DTOs/            # Data transfer objects
│   │   │   └── Common/          # Interfaces, mappings
│   │   ├── Infrastructure/       # Infrastructure Layer
│   │   │   ├── Data/            # DbContext, Migrations
│   │   │   ├── Repositories/    # Repository implementations
│   │   │   ├── Services/        # External services
│   │   │   └── Middleware/      # Custom middleware
│   │   └── Presentation/        # Presentation Layer
│   │       └── API/             # Controllers, Program.cs
│   └── CRM.sln
│
└── frontend/
    └── src/
        └── app/
            ├── core/             # Core services, guards
            ├── features/         # Feature modules
            └── shared/          # Shared components
```

---

## 🔐 Authentication / المصادقة

<div dir="rtl">

المشروع يستخدم **JWT (JSON Web Tokens)** للمصادقة:

- **Token Expiration**: 120 دقيقة
- **Password Hashing**: BCrypt
- **Roles**: Admin, Manager, Employee, ITSupport

</div>

The project uses **JWT (JSON Web Tokens)** for authentication:

- **Token Expiration**: 120 minutes
- **Password Hashing**: BCrypt
- **Roles**: Admin, Manager, Employee, ITSupport

---

## 📊 API Endpoints / نقاط النهاية

<div dir="rtl">

### Authentication
- `POST /api/auth/login` - تسجيل الدخول
- `POST /api/auth/register` - إنشاء حساب جديد
- `GET /api/auth/profile` - معلومات المستخدم

### Exceptions
- `GET /api/exceptions` - قائمة الاستثناءات
- `POST /api/exceptions` - إنشاء استثناء جديد
- `GET /api/exceptions/{id}` - تفاصيل استثناء
- `PUT /api/exceptions/{id}` - تحديث استثناء
- `DELETE /api/exceptions/{id}` - حذف استثناء

### Users
- `GET /api/users` - قائمة المستخدمين
- `GET /api/users/{id}` - تفاصيل مستخدم

### Customers
- `GET /api/customers` - قائمة العملاء
- `POST /api/customers` - إضافة عميل جديد

### Dashboard
- `GET /api/dashboard` - إحصائيات لوحة التحكم

</div>

### Authentication
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Register new account
- `GET /api/auth/profile` - User profile

### Exceptions
- `GET /api/exceptions` - List exceptions
- `POST /api/exceptions` - Create exception
- `GET /api/exceptions/{id}` - Exception details
- `PUT /api/exceptions/{id}` - Update exception
- `DELETE /api/exceptions/{id}` - Delete exception

### Users
- `GET /api/users` - List users
- `GET /api/users/{id}` - User details

### Customers
- `GET /api/customers` - List customers
- `POST /api/customers` - Add new customer

### Dashboard
- `GET /api/dashboard` - Dashboard statistics

---

## 🤖 AI Integration / تكامل الذكاء الاصطناعي

<div dir="rtl">

المشروع متكامل مع **N8N** للحصول على توصيات ذكية لمعالجة الاستثناءات:

- عند إنشاء استثناء جديد، يتم إرسال طلب إلى N8N webhook
- N8N يعالج الاستثناء ويعيد توصيات AI
- التوصيات تُحفظ في قاعدة البيانات ويمكن عرضها في الواجهة

</div>

The project integrates with **N8N** for AI-powered exception resolution recommendations:

- When a new exception is created, a request is sent to N8N webhook
- N8N processes the exception and returns AI recommendations
- Recommendations are saved in the database and can be displayed in the UI

---

## 🧪 Testing / الاختبار

<div dir="rtl">

### اختبار API من Swagger

1. افتح `http://localhost:5000/swagger`
2. جرب `POST /api/auth/login`:
   ```json
   {
     "username": "admin",
     "password": "Admin@123"
   }
   ```
3. انسخ الـ Token
4. اضغط على "Authorize" في Swagger
5. أدخل: `Bearer {your-token}`
6. جرب باقي الـ endpoints

</div>

### Testing API from Swagger

1. Open `http://localhost:5000/swagger`
2. Try `POST /api/auth/login`:
   ```json
   {
     "username": "admin",
     "password": "Admin@123"
   }
   ```
3. Copy the Token
4. Click "Authorize" in Swagger
5. Enter: `Bearer {your-token}`
6. Try other endpoints

---

## 📝 Default Credentials / بيانات الدخول الافتراضية

<div dir="rtl">

- **Username**: `admin`
- **Password**: `Admin@123`
- **Role**: `Admin`

> ⚠️ **تحذير**: غيّر كلمة المرور الافتراضية في بيئة الإنتاج!

</div>

- **Username**: `admin`
- **Password**: `Admin@123`
- **Role**: `Admin`

> ⚠️ **Warning**: Change the default password in production environment!

---

## 🐛 Troubleshooting / حل المشاكل

<div dir="rtl">

### مشكلة: "Cannot open database"
- تأكد من أن SQL Server يعمل
- تحقق من `ConnectionString` في `appsettings.json`
- تأكد من تطبيق Migrations

### مشكلة: CORS Error
- تأكد من أن الباك إند يعمل على `http://localhost:5000`
- تحقق من إعدادات CORS في `Program.cs`

### مشكلة: "JWT key is missing"
- حدّث `Jwt:Key` في `appsettings.json` بمفتاح سري قوي

</div>

### Issue: "Cannot open database"
- Ensure SQL Server is running
- Check `ConnectionString` in `appsettings.json`
- Make sure Migrations are applied

### Issue: CORS Error
- Ensure backend is running on `http://localhost:5000`
- Check CORS settings in `Program.cs`

### Issue: "JWT key is missing"
- Update `Jwt:Key` in `appsettings.json` with a strong secret key

---

## 📚 Documentation / التوثيق

<div dir="rtl">

- [Backend Requirements (AR)](backend/REQUIREMENTS_AR.md)
- [Frontend Requirements (AR)](frontend/REQUIREMENTS_AR.md)
- [Migration Guide (AR)](backend/MIGRATION_GUIDE_AR.md)
- [Migration Commands](backend/MIGRATION_COMMANDS.md)

</div>

- [Backend Requirements (AR)](backend/REQUIREMENTS_AR.md)
- [Frontend Requirements (AR)](frontend/REQUIREMENTS_AR.md)
- [Migration Guide (AR)](backend/MIGRATION_GUIDE_AR.md)
- [Migration Commands](backend/MIGRATION_COMMANDS.md)

---

## 🤝 Contributing / المساهمة

<div dir="rtl">

نرحب بمساهماتكم! يرجى:

1. Fork المشروع
2. إنشاء branch جديد (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push إلى Branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

</div>

Contributions are welcome! Please:

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---



