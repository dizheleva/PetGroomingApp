# Pet Grooming App

A comprehensive ASP.NET Core web application for managing pet grooming services, appointments, and customer relationships. This application provides a complete solution for pet grooming salons to manage their business operations efficiently.

## 📋 Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database](#database)
- [User Roles](#user-roles)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)
- [Code Quality](#code-quality)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

### For Customers
- **Pet Management**: Register and manage multiple pets with detailed information (name, type, breed, size, age, gender, notes)
- **Appointment Booking**: Schedule grooming appointments with:
  - Multiple service selection
  - Groomer selection
  - Date and time selection with availability checking
  - Automatic price and duration calculation
  - Appointment status tracking (Pending, Confirmed, Completed, Canceled)
- **Service Browsing**: View available grooming services with descriptions, prices, and durations
- **Groomer Profiles**: Browse groomer profiles with contact information and specializations
- **Appointment History**: View past and upcoming appointments with full details

### For Administrators
- **Dashboard**: Comprehensive admin dashboard with overview statistics
- **User Management**: 
  - Create, edit, and view user accounts
  - Assign and manage user roles
  - Search and filter users
- **Appointment Management**:
  - View all appointments across all users
  - Edit appointment details
  - Change appointment status
  - Create appointments for unregistered customers
  - Automatic status updates (expired appointments)
- **Service Management**: Full CRUD operations for grooming services
- **Groomer Management**: Full CRUD operations for groomer profiles
- **Pagination & Search**: Advanced filtering and pagination on all list views

### Additional Features
- **Responsive Design**: Mobile-friendly interface with modern UI/UX
- **Role-Based Access Control**: Secure access based on user roles (Admin, User)
- **Automatic Status Updates**: Background service updates appointment statuses automatically
- **Image Management**: Support for pet and groomer profile images with default icons
- **Search & Filtering**: Advanced search functionality across all entities
- **Pagination**: Efficient data display with configurable page sizes (default: 6 items per page)

## 🛠 Technologies

### Backend
- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core MVC** - Web application framework
- **Entity Framework Core 8.0** - ORM for database operations
- **SQL Server** - Primary database
- **ASP.NET Core Identity** - Authentication and authorization
- **Razor Pages** - For Identity management

### Frontend
- **Bootstrap 5.3.3** - CSS framework
- **jQuery** - JavaScript library
- **Font Awesome** - Icon library
- **Custom CSS/SCSS** - Styled components and themes

### Architecture Patterns
- **Repository Pattern** - Data access abstraction
- **Service Layer Pattern** - Business logic separation
- **Dependency Injection** - Loose coupling
- **ViewModels** - Clean separation between models and views
- **MVC Areas** - Organized code structure (Admin, Identity)

### Testing
- **NUnit** - Testing framework
- **Moq** - Mocking framework
- **MockQueryable** - EF Core query mocking
- **Coverlet** - Code coverage tool

## 📁 Project Structure

```
PetGroomingApp/
├── PetGroomingApp.Data/              # Data access layer
│   ├── ApplicationDbContext.cs      # Database context
│   ├── Configuration/               # EF Core configurations
│   ├── Migrations/                   # Database migrations
│   ├── Repository/                   # Repository implementations
│   └── Seeding/                      # Data seeding logic
│
├── PetGroomingApp.Data.Models/       # Entity models
│   ├── ApplicationUser.cs
│   ├── Appointment.cs
│   ├── Pet.cs
│   ├── Groomer.cs
│   ├── Service.cs
│   └── Enums/                        # Enum definitions
│
├── PetGroomingApp.Services.Core/     # Business logic layer
│   ├── Services/                     # Service implementations
│   │   ├── AppointmentService.cs
│   │   ├── PetService.cs
│   │   ├── ServiceService.cs
│   │   └── GroomerService.cs
│   ├── Admin/                        # Admin-specific services
│   │   └── UserService.cs
│   └── Interfaces/                   # Service interfaces
│
├── PetGroomingApp.Web/               # Web application
│   ├── Controllers/                  # MVC controllers
│   ├── Views/                        # Razor views
│   ├── Areas/                        # MVC areas
│   │   ├── Admin/                    # Admin area
│   │   └── Identity/                 # Identity pages
│   ├── ViewComponents/               # Reusable view components
│   └── wwwroot/                      # Static files
│
├── PetGroomingApp.Web.ViewModels/    # View models
│   ├── Appointment/
│   ├── Pet/
│   ├── Service/
│   ├── Groomer/
│   └── Admin/
│
├── PetGroomingApp.Web.Infrastructure/ # Infrastructure layer
│   ├── Extensions/                   # Extension methods
│   ├── Middlewares/                  # Custom middlewares
│   └── Services/                     # Background services
│
├── PetGroomingApp.Services.Core.Tests/ # Unit tests
│   ├── AppointmentServiceTests.cs
│   ├── PetServiceTests.cs
│   ├── ServiceServiceTests.cs
│   └── GroomerServiceTests.cs
│
└── PetGroomingApp.GCommon/           # Shared constants and utilities
```

## 🚀 Getting Started

### Prerequisites

- **.NET 8.0 SDK** or later
- **SQL Server** (LocalDB, Express, or full version)
- **Visual Studio 2022** or **Visual Studio Code** (recommended)
- **Git** for version control

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/PetGroomingApp.git
   cd PetGroomingApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database connection**
   
   Update `appsettings.json` with your SQL Server connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=PetGroomingApp;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   cd PetGroomingApp.Web
   dotnet ef database update --project ../PetGroomingApp.Data
   ```

5. **Seed initial data** (optional)
   
   The application automatically seeds default users and roles on first startup.

6. **Run the application**
   ```bash
   dotnet run --project PetGroomingApp.Web
   ```

7. **Access the application**
   
   Open your browser and navigate to:
   - **Home**: `https://localhost:7199` (or the port shown in console)
   - **Admin Dashboard**: `https://localhost:7199/Admin`

## ⚙️ Configuration

### Default Test Users

The application seeds the following test users:

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@example.com` | `Admin123!` |
| User | `test@example.com` | `Test123!` |

These can be configured in `appsettings.json`:

```json
{
  "UserSeed": {
    "TestUser": {
      "Email": "test@example.com",
      "Password": "Test123!"
    },
    "TestAdmin": {
      "Email": "admin@example.com",
      "Password": "Admin123!"
    }
  }
}
```

### Identity Configuration

Identity settings can be customized in `appsettings.Development.json`:

```json
{
  "IdentityConfig": {
    "SignIn": {
      "RequireConfirmedEmail": false,
      "RequireConfirmedAccount": false,
      "RequireConfirmedPhoneNumber": false
    },
    "Password": {
      "RequiredLength": 3,
      "RequireNonAlphanumeric": false,
      "RequireDigit": false,
      "RequireLowercase": false,
      "RequireUppercase": false,
      "RequiredUniqueChars": 0
    }
  }
}
```

### Pagination Settings

Default page size is set to 6 items per page. This can be modified in:
- `PaginationHelper.cs`
- Individual controller actions

## 🗄️ Database

### Entity Models

- **ApplicationUser**: Extended Identity user with FirstName, LastName
- **Pet**: Pet information (Name, Type, Breed, Size, Age, Gender, ImageUrl, Notes)
- **Groomer**: Groomer profiles (FirstName, LastName, JobTitle, PhoneNumber, Description, ImageUrl)
- **Service**: Grooming services (Name, Description, Price, Duration, ImageUrl)
- **Appointment**: Appointment records (AppointmentTime, Status, TotalPrice, TotalDuration, Notes)
- **AppointmentService**: Many-to-many relationship between Appointments and Services

### Database Migrations

To create a new migration:
```bash
dotnet ef migrations add MigrationName --project PetGroomingApp.Data --startup-project PetGroomingApp.Web
```

To apply migrations:
```bash
dotnet ef database update --project PetGroomingApp.Data --startup-project PetGroomingApp.Web
```

## 👥 User Roles

### Admin
- Full access to all features
- User management (create, edit, view, assign roles)
- Appointment management (view all, edit, change status)
- Service management (CRUD operations)
- Groomer management (CRUD operations)
- Access to admin dashboard

### User
- Register and manage pets
- Create and manage appointments
- View services and groomers
- View own appointment history
- Manage account settings

## 🔌 API Endpoints

The application includes API controllers for AJAX operations:

### Appointment API
- `GET /api/appointment/available-times?groomerId={id}&duration={minutes}` - Get available appointment times
- `GET /api/appointment/totals?serviceIds={ids}` - Calculate total price and duration

### Groomer API
- `GET /api/groomer/available?appointmentTime={datetime}&duration={minutes}` - Get available groomers

### Pet API
- `GET /api/pet/by-user?userId={id}` - Get pets by user ID

## 🧪 Testing

### Running Tests

```bash
cd PetGroomingApp.Services.Core.Tests
dotnet test
```

### Test Coverage

The project includes unit tests for:
- ✅ AppointmentService (11 tests)
- ✅ PetService (10 tests)
- ✅ ServiceService (10 tests)
- ✅ GroomerService (9 tests)

**Total: 40 unit tests** with 100% pass rate

### Code Coverage

To generate code coverage report:
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## 📊 Code Quality

### Current Status

- **Overall Rating**: 8.5/10
- **Logging**: ✅ Structured logging with ILogger<T>
- **Error Handling**: ✅ Specific exception types and user-friendly messages
- **Security**: ✅ CSRF protection on all POST actions
- **Performance**: ✅ Optimized queries with eager loading
- **Testing**: ✅ 40 unit tests covering core services

### Best Practices Implemented

- ✅ Repository Pattern for data access
- ✅ Service Layer for business logic
- ✅ Dependency Injection throughout
- ✅ ViewModels for clean separation
- ✅ Structured logging
- ✅ Comprehensive error handling
- ✅ Role-based authorization
- ✅ Input validation
- ✅ Anti-forgery tokens

## 🎨 UI/UX Features

- **Modern Design**: Clean, professional interface with gradient themes
- **Responsive Layout**: Mobile-friendly design
- **Icon Integration**: Font Awesome icons throughout
- **Default Images**: Beautiful default icons for pets and groomers
- **Pagination**: User-friendly pagination controls
- **Search Functionality**: Real-time search across entities
- **Form Validation**: Client-side and server-side validation
- **Toast Notifications**: User feedback for actions

## 🔒 Security Features

- **Authentication**: ASP.NET Core Identity
- **Authorization**: Role-based access control
- **CSRF Protection**: Anti-forgery tokens on all forms
- **Input Validation**: Server-side and client-side validation
- **SQL Injection Prevention**: Parameterized queries via EF Core
- **XSS Protection**: HTML encoding by default
- **Secure Routing**: Protected admin routes

## 📝 Key Functionalities

### Appointment System
- **Smart Scheduling**: Automatic conflict detection
- **Dynamic Pricing**: Automatic calculation based on selected services
- **Status Management**: Automatic status updates via background service
- **Timezone Handling**: Proper UTC/Local time conversion

### Background Services
- **AppointmentStatusUpdateService**: Automatically updates appointment statuses:
  - Pending → Canceled (if time passed)
  - Confirmed → Completed (if time passed)

## 🐛 Known Issues

None currently. The application is production-ready.

## 🚧 Future Enhancements

Potential improvements for future versions:
- Email notifications for appointments
- SMS reminders
- Payment integration
- Customer reviews and ratings
- Mobile app
- Advanced reporting and analytics
- Multi-language support

## 📚 Documentation

Additional documentation:
- `CODE_QUALITY_ANALYSIS.md` - Detailed code quality analysis
- `PROJECT_ANALYSIS.md` - Project requirements analysis

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Dilyana Dizheleva**

- GitHub: [@dizheleva](https://github.com/dizheleva)