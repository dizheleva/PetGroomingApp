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
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;
    using ServiceService = Services.ServiceService;

    [TestFixture]
    public class ServiceServiceTests
    {
        private Mock<IServiceRepository> _serviceRepositoryMock;
        private IServiceService _service;

        [SetUp]
        public void Setup()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>(MockBehavior.Strict);
            _service = new ServiceService(_serviceRepositoryMock.Object);
        }

        [Test]
        public async Task AddAsync_CreatesService_WhenValid()
        {
            // Arrange
            var model = new ServiceFormViewModel
            {
                Name = "Haircut",
                Description = "Full haircut service",
                ImageUrl = "image.jpg",
                Duration = "01:00:00",
                Price = 50m
            };

            Service? savedService = null;
            _serviceRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Service>()))
                .Callback<Service>(s => savedService = s)
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddAsync(model);

            // Assert
            Assert.IsNotNull(savedService);
            Assert.AreEqual(model.Name, savedService.Name);
            Assert.AreEqual(model.Description, savedService.Description);
            Assert.AreEqual(model.Price, savedService.Price);
            Assert.AreEqual(TimeSpan.FromHours(1), savedService.Duration);
            _serviceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Service>()), Times.Once);
        }

        [Test]
        public void AddAsync_ThrowsArgumentNullException_WhenModelIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _service.AddAsync(null!));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsService_WhenExists()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var service = new Service
            {
                Id = serviceId,
                Name = "Haircut",
                Description = "Full haircut",
                ImageUrl = "image.jpg",
                Duration = TimeSpan.FromHours(1),
                Price = 50m,
                IsDeleted = false
            };

            var services = new List<Service> { service };
            var mockSet = services.BuildMock();
            _serviceRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetByIdAsync(serviceId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(service.Name, result.Name);
            Assert.AreEqual(service.Description, result.Description);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenServiceNotFound()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var services = new List<Service>();
            var mockSet = services.BuildMock();
            _serviceRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetByIdAsync(serviceId.ToString());

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenInvalidGuid()
        {
            // Act
            var result = await _service.GetByIdAsync("invalid-guid");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedServices()
        {
            // Arrange
            var services = new List<Service>
            {
                new Service { Id = Guid.NewGuid(), Name = "Service1", IsDeleted = false },
                new Service { Id = Guid.NewGuid(), Name = "Service2", IsDeleted = false },
                new Service { Id = Guid.NewGuid(), Name = "Service3", IsDeleted = true }
            };
            var mockSet = services.BuildMock();
            _serviceRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.AreEqual(2, resultList.Count);
            Assert.IsTrue(resultList.All(s => s.Name == "Service1" || s.Name == "Service2"));
        }

        [Test]
        public async Task EditAsync_ReturnsTrue_WhenServiceExists()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var service = new Service
            {
                Id = serviceId,
                Name = "OldName",
                IsDeleted = false
            };

            var model = new ServiceFormViewModel
            {
                Id = serviceId.ToString(),
                Name = "NewName",
                Description = "New Description",
                ImageUrl = "new-image.jpg",
                Duration = "02:00:00",
                Price = 100m
            };

            _serviceRepositoryMock
                .Setup(r => r.GetByIdAsync(serviceId))
                .ReturnsAsync(service);

            _serviceRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Service>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.EditAsync(serviceId.ToString(), model);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(model.Name, service.Name);
            Assert.AreEqual(model.Description, service.Description);
            Assert.AreEqual(TimeSpan.FromHours(2), service.Duration);
            _serviceRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Service>()), Times.Once);
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenServiceNotFound()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var model = new ServiceFormViewModel 
            { 
                Name = "NewName",
                ImageUrl = "image.jpg",
                Duration = "01:00:00",
                Price = 50m
            };

            _serviceRepositoryMock
                .Setup(r => r.GetByIdAsync(serviceId))
                .ReturnsAsync((Service?)null);

            // Act
            var result = await _service.EditAsync(serviceId.ToString(), model);

            // Assert
            Assert.IsFalse(result);
            _serviceRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Service>()), Times.Never);
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenModelIsNull()
        {
            // Act
            var result = await _service.EditAsync(Guid.NewGuid().ToString(), null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CalculateTotalsAsync_ReturnsCorrectTotals()
        {
            // Arrange
            var serviceId1 = Guid.NewGuid();
            var serviceId2 = Guid.NewGuid();
            var services = new List<Service>
            {
                new Service { Id = serviceId1, Duration = TimeSpan.FromMinutes(30), Price = 25m, IsDeleted = false },
                new Service { Id = serviceId2, Duration = TimeSpan.FromMinutes(45), Price = 35m, IsDeleted = false }
            };
            var mockSet = services.BuildMock();
            _serviceRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.CalculateTotalsAsync(new List<string> 
            { 
                serviceId1.ToString(), 
                serviceId2.ToString() 
            });

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(75, result.TotalDuration); // 30 + 45 minutes
            Assert.AreEqual(60m, result.TotalPrice); // 25 + 35
        }

        [Test]
        public async Task CalculateTotalsAsync_ReturnsZero_WhenNoServices()
        {
            // Act
            var result = await _service.CalculateTotalsAsync(new List<string>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.TotalDuration);
            Assert.AreEqual(0m, result.TotalPrice);
        }

        [Test]
        public async Task GetTotalDurationAsync_ReturnsZero_WhenInvalidGuids()
        {
            // Act
            var result = await _service.GetTotalDurationAsync(new List<string> { "invalid-guid" });

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}

