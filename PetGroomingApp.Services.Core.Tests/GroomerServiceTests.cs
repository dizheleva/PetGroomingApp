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
    using PetGroomingApp.Web.ViewModels.Groomer;
    using GroomerService = Services.GroomerService;

    [TestFixture]
    public class GroomerServiceTests
    {
        private Mock<IGroomerRepository> _groomerRepositoryMock;
        private IGroomerService _service;

        [SetUp]
        public void Setup()
        {
            _groomerRepositoryMock = new Mock<IGroomerRepository>(MockBehavior.Strict);
            _service = new GroomerService(_groomerRepositoryMock.Object);
        }

        [Test]
        public async Task AddAsync_CreatesGroomer_WhenValid()
        {
            // Arrange
            var model = new GroomerFormViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                JobTitle = "Senior Groomer",
                ImageUrl = "image.jpg",
                Description = "Experienced groomer"
            };

            Groomer? savedGroomer = null;
            _groomerRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Groomer>()))
                .Callback<Groomer>(g => savedGroomer = g)
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddAsync(model);

            // Assert
            Assert.IsNotNull(savedGroomer);
            Assert.AreEqual(model.FirstName, savedGroomer.FirstName);
            Assert.AreEqual(model.LastName, savedGroomer.LastName);
            Assert.AreEqual(model.JobTitle, savedGroomer.JobTitle);
            Assert.AreEqual(model.Description, savedGroomer.Description);
            _groomerRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Groomer>()), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsGroomer_WhenExists()
        {
            // Arrange
            var groomerId = Guid.NewGuid();
            var groomer = new Groomer
            {
                Id = groomerId,
                FirstName = "John",
                LastName = "Doe",
                JobTitle = "Senior Groomer",
                ImageUrl = "image.jpg",
                Description = "Experienced",
                IsDeleted = false
            };

            var groomers = new List<Groomer> { groomer };
            var mockSet = groomers.BuildMock();
            _groomerRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetByIdAsync(groomerId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual($"{groomer.FirstName} {groomer.LastName}", result.Name);
            Assert.AreEqual(groomer.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenGroomerNotFound()
        {
            // Arrange
            var groomerId = Guid.NewGuid();
            var groomers = new List<Groomer>();
            var mockSet = groomers.BuildMock();

            _groomerRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetByIdAsync(groomerId.ToString());

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsOnlyNonDeletedGroomers()
        {
            // Arrange
            var groomers = new List<Groomer>
            {
                new Groomer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", IsDeleted = false },
                new Groomer { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", IsDeleted = false },
                new Groomer { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Johnson", IsDeleted = true }
            };
            var mockSet = groomers.BuildMock();
            _groomerRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.AreEqual(2, resultList.Count);
            Assert.IsTrue(resultList.Any(g => g.Name.Contains("John")));
            Assert.IsTrue(resultList.Any(g => g.Name.Contains("Jane")));
        }

        [Test]
        public async Task EditAsync_ReturnsTrue_WhenGroomerExists()
        {
            // Arrange
            var groomerId = Guid.NewGuid();
            var groomer = new Groomer
            {
                Id = groomerId,
                FirstName = "OldFirst",
                LastName = "OldLast",
                JobTitle = "OldTitle",
                IsDeleted = false
            };

            var model = new GroomerFormViewModel
            {
                Id = groomerId.ToString(),
                FirstName = "NewFirst",
                LastName = "NewLast",
                JobTitle = "NewTitle",
                Description = "New Bio",
                ImageUrl = "new-image.jpg"
            };

            _groomerRepositoryMock
                .Setup(r => r.GetByIdAsync(groomerId))
                .ReturnsAsync(groomer);

            _groomerRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Groomer>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.EditAsync(groomerId.ToString(), model);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(model.FirstName, groomer.FirstName);
            Assert.AreEqual(model.LastName, groomer.LastName);
            Assert.AreEqual(model.JobTitle, groomer.JobTitle);
            _groomerRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Groomer>()), Times.Once);
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenGroomerNotFound()
        {
            // Arrange
            var groomerId = Guid.NewGuid();
            var model = new GroomerFormViewModel { FirstName = "NewFirst" };

            _groomerRepositoryMock
                .Setup(r => r.GetByIdAsync(groomerId))
                .ReturnsAsync((Groomer?)null);

            // Act
            var result = await _service.EditAsync(groomerId.ToString(), model);

            // Assert
            Assert.IsFalse(result);
            _groomerRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Groomer>()), Times.Never);
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
        public async Task GetForEditByIdAsync_ReturnsGroomer_WhenExists()
        {
            // Arrange
            var groomerId = Guid.NewGuid();
            var groomer = new Groomer
            {
                Id = groomerId,
                FirstName = "John",
                LastName = "Doe",
                JobTitle = "Senior Groomer",
                Description = "Experienced",
                ImageUrl = "image.jpg",
                IsDeleted = false
            };

            var groomers = new List<Groomer> { groomer };
            var mockSet = groomers.BuildMock();
            _groomerRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetForEditByIdAsync(groomerId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(groomer.FirstName, result.FirstName);
            Assert.AreEqual(groomer.LastName, result.LastName);
            Assert.AreEqual(groomer.JobTitle, result.JobTitle);
        }

        [Test]
        public async Task GetForEditByIdAsync_ReturnsNull_WhenGroomerNotFound()
        {
            // Arrange
            var groomerId = Guid.NewGuid();
            var groomers = new List<Groomer>();
            var mockSet = groomers.BuildMock();

            _groomerRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetForEditByIdAsync(groomerId.ToString());

            // Assert
            Assert.IsNull(result);
        }
    }
}

