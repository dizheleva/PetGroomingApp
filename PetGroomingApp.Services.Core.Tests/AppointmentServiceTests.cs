namespace PetGroomingApp.Services.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MockQueryable;
    using Moq;
    using NUnit.Framework;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Models.Enums;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;
    using AppointmentService = Services.AppointmentService;

    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private IAppointmentService _service;

        [SetUp]
        public void Setup()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>(MockBehavior.Strict);
            _service = new AppointmentService(_appointmentRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAsync_ReturnsEmptyString_WhenNoServicesSelected()
        {
            var model = new AppointmentUserFormViewModel
            {
                SelectedServiceIds = new List<Guid>(),
            };

            var result = await _service.CreateAsync(model, "userId");
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public async Task CreateAsync_ReturnsEmptyString_WhenRepoReturnsNoServices()
        {
            // Arrange
            var model = new AppointmentUserFormViewModel
            {
                Id = Guid.NewGuid(),
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                Notes = "notes",
                SelectedGroomerId = Guid.NewGuid(),
                SelectedServiceIds = new List<Guid> { Guid.NewGuid() }, 
                Status = "Pending",
                TotalDuration = TimeSpan.FromMinutes(60),
                TotalPrice = 100m
            };
            
            _appointmentRepositoryMock
                .Setup(r => r.GetAppointmentServicesByIds(model.SelectedServiceIds))
                .ReturnsAsync(new List<Service>()); 
                        
            var result = await _service.CreateAsync(model, "userId");

            Assert.AreEqual(string.Empty, result);

            _appointmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Appointment>()), Times.Never);
        }


        [Test]
        public async Task CreateAsync_ReturnsAppointmentId_WhenValid()
        {
            var serviceId = Guid.NewGuid();
            var model = new AppointmentUserFormViewModel
            {
                Id = Guid.NewGuid(),
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                Notes = "notes",
                SelectedGroomerId = Guid.NewGuid(),
                SelectedServiceIds = new List<Guid> { serviceId },
                Status = "Pending",
                TotalDuration = TimeSpan.FromMinutes(60),
                TotalPrice = 100m
            };
            var services = new List<Service>
                {
                    new Service
                    {
                        Id = serviceId,
                        Name = "Haircut",
                        Price = 100m,
                        Duration = TimeSpan.FromMinutes(60)
                    }
                };
            _appointmentRepositoryMock
                .Setup(r => r.GetAppointmentServicesByIds(model.SelectedServiceIds))
                .ReturnsAsync(services);

            _appointmentRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(model, "userId");

            Assert.IsTrue(Guid.TryParse(result, out _));
            _appointmentRepositoryMock.Verify(r => r.AddAsync(It.Is<Appointment>(a =>
                a.UserId == "userId" &&
                a.Status == AppointmentStatus.Pending &&
                a.TotalPrice == 100m &&
                a.Duration.TotalMinutes == 60 &&
                a.AppointmentServices.Count == 1
            )), Times.Once);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenIdInvalidOrUserIdNull()
        {
            var result = await _service.GetDetailsAsync("not-a-guid", "user");
            Assert.IsNull(result);

            result = await _service.GetDetailsAsync(Guid.NewGuid().ToString(), null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDetails_WhenFound()
        {
            var appointmentId = Guid.NewGuid();
            var userId = "user123";

            var service = new Service
            {
                Id = Guid.NewGuid(),
                Name = "Haircut",
                Price = 55,
                Duration = TimeSpan.FromMinutes(45),
                ImageUrl = "https://example.com/image.jpg"
            };

            var appointment = new Appointment
            {
                Id = appointmentId,
                AppointmentTime = DateTime.UtcNow.AddDays(1),
                Duration = TimeSpan.FromMinutes(45),
                Notes = "Haircut only",
                UserId = userId,
                Pet = new Pet 
                { 
                    Name = "Buddy", 
                    Breed = "Golden Retriever",
                    Type = PetType.Dog,
                    Size = PetSize.Large,
                    Age = 3,
                    ImageUrl = "https://example.com/pet.jpg"
                },
                Groomer = new Groomer
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    ImageUrl = "https://example.com/groomer.jpg",
                    JobTitle = "Senior Groomer",
                    PhoneNumber = "555-1234"
                },
                Status = AppointmentStatus.Pending,
                TotalPrice = 55,
                AppointmentServices = new List<Data.Models.AppointmentService>
                {
                    new Data.Models.AppointmentService
                    {
                        ServiceId = service.Id,
                        AppointmentId = appointmentId
                    }
                },
                User = new ApplicationUser
                {
                    FirstName = "John",
                    LastName = "Smith"
                }
            };

            List<Appointment> appointments = new List<Appointment> { appointment };
            var mockSet = appointments.BuildMock();

            _appointmentRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            _appointmentRepositoryMock
                .Setup(r => r.GetAppointmentServicesNamesByIdsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(new List<string> { service.Name });

            var result = await _service.GetDetailsAsync(appointmentId.ToString(), userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(appointmentId.ToString(), result.Id);
            Assert.AreEqual("Buddy", result.PetName);
            Assert.AreEqual("Jane Doe", result.GroomerName);
            Assert.AreEqual("John Smith", result.OwnerName);
            Assert.AreEqual("Haircut", result.Services.First());
            Assert.AreEqual(AppointmentStatus.Pending.ToString(), result.Status);
            Assert.AreEqual(55, result.Price);
        }



        [Test]
        public async Task EditAsync_ReturnsFalse_WhenIdInvalidOrUserIdNull()
        {
            var model = new AppointmentUserFormViewModel();
            var result = await _service.EditAsync("not-a-guid", model, "user");
            Assert.IsFalse(result);

            result = await _service.EditAsync(Guid.NewGuid().ToString(), model, null);
            Assert.IsFalse(result);
        }

        [Test]
        public void EditAsync_Throws_WhenAppointmentCompletedOrCanceled()
        {
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Completed
            };
            _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);

            var model = new AppointmentUserFormViewModel { AppointmentTime = DateTime.UtcNow.AddDays(1) };
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EditAsync(appointmentId.ToString(), model, "user"));
        }

        [Test]
        public void EditAsync_Throws_WhenAppointmentTimeInPast()
        {
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending,
                UserId = "user"
            };
            _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);

            var model = new AppointmentUserFormViewModel
            {
                AppointmentTime = DateTime.UtcNow.AddMinutes(-10)
            };
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EditAsync(appointmentId.ToString(), model, "user"));
        }

        [Test]
        public void EditAsync_Throws_WhenUserIsNotOwner()
        {
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending,
                UserId = "otheruser"
            };
            _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);

            var model = new AppointmentUserFormViewModel
            {
                AppointmentTime = DateTime.UtcNow.AddMinutes(10)
            };
            Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.EditAsync(appointmentId.ToString(), model, "user"));
        }

        [Test]
        public async Task EditAsync_UpdatesAndReturnsTrue_WhenValid()
        {
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment
            {
                Status = AppointmentStatus.Pending,
                UserId = "user"
            };
            _appointmentRepositoryMock.Setup(r => r.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(r => r.UpdateAsync(appointment)).ReturnsAsync(true);

            var model = new AppointmentUserFormViewModel
            {
                AppointmentTime = DateTime.UtcNow.AddMinutes(10),
                SelectedGroomerId = Guid.NewGuid(),
                SelectedPetId = Guid.NewGuid(),
                Notes = "Updated notes",
                TotalPrice = 99.9m,
                TotalDuration = TimeSpan.FromMinutes(30),
                SelectedServiceIds = new List<Guid> { Guid.NewGuid() },
            };

            var result = await _service.EditAsync(appointmentId.ToString(), model, "user");
            Assert.IsTrue(result);
            _appointmentRepositoryMock.Verify(r => r.UpdateAsync(appointment), Times.Once);
            Assert.AreEqual("Updated notes", appointment.Notes);
            Assert.AreEqual(99.9m, appointment.TotalPrice);
        }
    }
}