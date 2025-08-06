# PetGroomingApp

PetGroomingApp is a C# application designed to manage and streamline pet grooming services. This project provides features for appointment scheduling, user and manager administration, and service tracking within a pet grooming business context.

## Features

- **Appointment Management:** Schedule, edit, and cancel grooming appointments. Supports overlapping checks, status tracking (pending, completed, canceled), and flexible service selection.
- **User & Role Management:** Admin features for managing users and assigning roles using ASP.NET Identity.
- **Service Management:** Select and assign multiple grooming services per appointment, track durations and pricing.
- **Manager Dashboard:** Special views and actions for managers, including user/appointment overviews and administration tools.

## Technologies Used

- **C# / .NET**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **Moq & NUnit** for testing
- **Repository Pattern** for data access
- **ViewModels** for clean separation between backend and presentation layers

## Project Structure

- `PetGroomingApp.Services.Core/Services/AppointmentService.cs` - Business logic for appointment handling.
- `PetGroomingApp.Services.Core/Admin/Services/UserService.cs` - User and role management logic.
- `PetGroomingApp.Data.Models/` - Entity and enum definitions.
- `PetGroomingApp.Data.Repository.Interfaces/` - Data access interfaces.
- `PetGroomingApp.Web.ViewModels/` - ViewModel classes for UI operations.

## License

This project is licensed under the MIT License.

## Contributing

Contributions are welcome! Please fork the repo and submit a pull request.

## Contact

[GitHub Profile](https://github.com/dizheleva)
