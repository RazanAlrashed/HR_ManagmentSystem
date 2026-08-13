# HR Management System

A web-based **Human Resource Management System (HRMS)** developed using **ASP.NET Core MVC** to help organizations manage employee information, departments, designations, leave applications, salary information, and administrative HR operations through a centralized system.

## 📌 Overview

The HR Management System provides separate functionality for **administrators and employees**.

Administrators can manage employee records, departments, designations, leave applications, and salary information, while authenticated employees can access their profiles and submit and track leave applications.

The system follows the **MVC architecture** and uses **Entity Framework Core** for database operations with **SQL Server** as the database.

## ✨ Features

### 👨‍💼 Admin Features

* Secure administrator authentication and authorization
* Employee management

  * Add employees
  * View employee records
  * Edit employee information
  * Delete employee records
* Department management

  * Add departments
  * Edit departments
  * Delete departments
* Designation management

  * Add designations
  * Edit designations
  * Delete designations
* View employee information including:

  * Employee number
  * Name
  * Email
  * Contact number
  * Department
  * Designation
  * Salary
  * Bank information
  * Nationality
  * Address
* Leave application management
* Approve or reject employee leave requests
* Salary and employee reports
* Export employee and salary information to Excel
* Generate PDF-based reports

### 👤 Employee Features

* Secure user authentication
* Employee dashboard
* View personal profile
* Submit leave applications
* View submitted leave applications
* Track leave application status

## 🛠️ Technologies Used

| Technology                  | Purpose                          |
| --------------------------- | -------------------------------- |
| **ASP.NET Core 9.0**        | Web application framework        |
| **C#**                      | Backend programming language     |
| **ASP.NET Core MVC**        | Application architecture         |
| **Entity Framework Core**   | ORM and database access          |
| **SQL Server**              | Relational database              |
| **ASP.NET Core Identity**   | Authentication and authorization |
| **Razor Views**             | Server-side UI                   |
| **HTML / CSS / JavaScript** | Frontend development             |
| **EPPlus**                  | Excel report generation          |
| **iText 7**                 | PDF generation                   |

The project targets **.NET 9.0** and includes Entity Framework Core SQL Server and ASP.NET Core Identity packages.

## 🏗️ Architecture

The application follows the **Model–View–Controller (MVC)** architectural pattern.

```text
HR_ManagmentSystem
│
├── Controllers
│   ├── AdminController.cs
│   ├── DashboardController.cs
│   ├── HomeController.cs
│   ├── UserAuthenticationController.cs
│   └── UserController.cs
│
├── Models
│   ├── BaseModel
│   ├── DTO
│   ├── Domain
│   └── ErrorViewModel.cs
│
├── Repositories
│   └── Abstract / Repository implementations
│
├── Migrations
│   └── Entity Framework Core migrations
│
├── Views
│   ├── Admin
│   ├── Dashboard
│   ├── Home
│   └── User
│
├── wwwroot
│   ├── CSS
│   ├── JavaScript
│   └── Static assets
│
├── Program.cs
├── appsettings.json
└── HR_ManagmentSystem.csproj
```

The repository is organized into controllers, models, repositories, migrations, views, and static web assets.

## 🔐 Authentication & Authorization

The system uses **ASP.NET Core Identity** for authentication and role-based authorization.

Administrative operations are protected using an administrator role, while employee functionality requires an authenticated user.

For example, administrative functionality is restricted to users with the `admin` role, while employee functionality requires authentication.

## 🗄️ Database

The application uses:

* **SQL Server**
* **Entity Framework Core**
* **EF Core Migrations**
* **ASP.NET Core Identity**

The database context and application user are implemented under the domain models, while database migrations are maintained in the `Migrations` directory.

## 📊 Reporting

The system provides HR reporting functionality for employee and salary information.

Employee data can be exported to **Excel** using EPPlus, including information such as employee number, name, department, designation, salary, bank information, nationality, and address.

The project also includes **iText 7** for PDF generation.

## 🚀 Getting Started

### Prerequisites

Before running the project, make sure you have:

* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* SQL Server
* Visual Studio 2022 or another compatible .NET IDE
* Git

### 1. Clone the Repository

```bash
git clone https://github.com/RazanAlrashed/HR_ManagmentSystem.git
cd HR_ManagmentSystem
```

### 2. Configure the Database

Open:

```text
appsettings.json
```

Configure the SQL Server connection string according to your local environment.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HRManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Replace `YOUR_SERVER` with your SQL Server instance.

### 3. Apply Database Migrations

Run:

```bash
dotnet ef database update
```

If the Entity Framework CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Build the Project

```bash
dotnet build
```

### 6. Run the Application

```bash
dotnet run
```

The application will start using the ASP.NET Core development server.

You can also run the project directly from Visual Studio using the provided solution file:

```text
HR_ManagmentSystem.sln
```

## 🔄 Main Workflow

### Administrator Workflow

```text
Admin Login
     │
     ▼
Admin Dashboard
     │
     ├── Manage Employees
     │      ├── Add
     │      ├── Edit
     │      ├── View
     │      └── Delete
     │
     ├── Manage Departments
     │
     ├── Manage Designations
     │
     ├── Manage Leave Applications
     │      ├── Approve
     │      └── Reject
     │
     └── Generate Reports
            ├── Excel
            └── PDF
```

### Employee Workflow

```text
Employee Login
      │
      ▼
Employee Dashboard
      │
      ├── View Profile
      │
      └── Leave Management
             ├── Apply for Leave
             └── Track Application Status
```

The employee workflow is implemented through authenticated user functionality, including profile access and leave application tracking.

## 📁 Project Structure

| Folder / File               | Description                                         |
| --------------------------- | --------------------------------------------------- |
| `Controllers/`              | Handles application requests and business workflows |
| `Models/`                   | Contains domain models, DTOs, and base models       |
| `Repositories/`             | Repository layer for data access abstraction        |
| `Migrations/`               | Entity Framework Core database migrations           |
| `Views/`                    | Razor-based user interface                          |
| `wwwroot/`                  | CSS, JavaScript, images, and static assets          |
| `Program.cs`                | Application configuration and startup               |
| `appsettings.json`          | Application and database configuration              |
| `HR_ManagmentSystem.csproj` | Project dependencies and .NET configuration         |

## 🎯 Project Objectives

The main objectives of this project are to:

* Digitize HR management processes
* Centralize employee information
* Simplify employee record management
* Improve leave request management
* Provide role-based access to HR functionality
* Reduce manual HR record keeping
* Generate structured employee and salary reports
* Apply modern web development and database technologies

## 🔮 Future Improvements

Potential future enhancements include:

* Employee attendance tracking
* Payroll automation
* Email notifications for leave requests
* Advanced HR analytics dashboard
* Role management interface
* Search and filtering for employee records
* Responsive UI improvements
* RESTful API integration
* Cloud deployment
* Automated testing and CI/CD

