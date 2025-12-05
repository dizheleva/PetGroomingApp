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
    using PetGroomingApp.Web.ViewModels.Pet;
    using PetService = Services.PetService;

    [TestFixture]
    public class PetServiceTests
    {
        private Mock<IPetRepository> _petRepositoryMock;
        private IPetService _service;

        [SetUp]
        public void Setup()
        {
            _petRepositoryMock = new Mock<IPetRepository>(MockBehavior.Strict);
            _service = new PetService(_petRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAsync_ReturnsPetId_WhenValid()
        {
            // Arrange
            var model = new PetFormViewModel
            {
                Name = "Fluffy",
                Type = PetType.Dog,
                Breed = "Golden Retriever",
                Size = PetSize.Large,
                Gender = PetGender.Male,
                Age = 3,
                Notes = "Friendly dog"
            };
            var ownerId = "owner123";

            Pet? savedPet = null;
            _petRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Pet>()))
                .Callback<Pet>(p => savedPet = p)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(model, ownerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(savedPet);
            Assert.AreEqual(savedPet.Id.ToString(), result);
            Assert.AreEqual(model.Name, savedPet.Name);
            Assert.AreEqual(model.Type, savedPet.Type);
            Assert.AreEqual(ownerId, savedPet.OwnerId);
            _petRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Pet>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_UsesDefaultImageUrl_WhenImageUrlIsNull()
        {
            // Arrange
            var model = new PetFormViewModel
            {
                Name = "Fluffy",
                Type = PetType.Dog,
                Breed = "Golden Retriever",
                Size = PetSize.Medium,
                Gender = PetGender.Female,
                Age = 2,
                ImageUrl = null
            };
            var ownerId = "owner123";

            Pet? savedPet = null;
            _petRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Pet>()))
                .Callback<Pet>(p => savedPet = p)
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(model, ownerId);

            // Assert
            Assert.IsNotNull(savedPet);
            Assert.IsNotNull(savedPet.ImageUrl);
            Assert.IsNotEmpty(savedPet.ImageUrl);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsPetDetails_WhenPetExists()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var ownerId = "owner123";
            var pet = new Pet
            {
                Id = petId,
                Name = "Fluffy",
                Type = PetType.Dog,
                Breed = "Golden Retriever",
                Size = PetSize.Large,
                Gender = PetGender.Male,
                Age = 3,
                Notes = "Friendly",
                ImageUrl = "image.jpg",
                OwnerId = ownerId,
                IsDeleted = false
            };

            var pets = new List<Pet> { pet };
            var mockSet = pets.BuildMock();
            _petRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetDetailsAsync(petId.ToString(), ownerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pet.Name, result.Name);
            Assert.AreEqual(pet.Type, result.Type);
            Assert.AreEqual(pet.Breed, result.Breed);
        }

        [Test]
        public async Task GetDetailsAsync_ThrowsException_WhenPetNotFound()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var ownerId = "owner123";
            var pets = new List<Pet>();
            var mockSet = pets.BuildMock();
            _petRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.GetDetailsAsync(petId.ToString(), ownerId));
        }

        [Test]
        public async Task GetPetsByUserAsync_ReturnsOnlyUserPets()
        {
            // Arrange
            var userId = "user123";
            var pets = new List<Pet>
            {
                new Pet { Id = Guid.NewGuid(), Name = "Pet1", OwnerId = userId, IsDeleted = false },
                new Pet { Id = Guid.NewGuid(), Name = "Pet2", OwnerId = userId, IsDeleted = false },
                new Pet { Id = Guid.NewGuid(), Name = "Pet3", OwnerId = "otherUser", IsDeleted = false }
            };
            var mockSet = pets.BuildMock();
            _petRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.GetPetsByUserAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.AreEqual(2, resultList.Count);
            Assert.IsTrue(resultList.All(p => p != null && p.Name == "Pet1" || p.Name == "Pet2"));
        }

        [Test]
        public async Task EditAsync_ReturnsTrue_WhenPetExists()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var ownerId = "owner123";
            var pet = new Pet
            {
                Id = petId,
                Name = "OldName",
                OwnerId = ownerId,
                IsDeleted = false
            };

            var model = new PetFormViewModel
            {
                Id = petId.ToString(),
                Name = "NewName",
                Type = PetType.Cat,
                Breed = "Persian",
                Size = PetSize.Small,
                Gender = PetGender.Female,
                Age = 2
            };

            _petRepositoryMock
                .Setup(r => r.GetByIdAsync(petId))
                .ReturnsAsync(pet);

            _petRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Pet>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.EditAsync(petId.ToString(), model, ownerId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(model.Name, pet.Name);
            _petRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Pet>()), Times.Once);
        }

        [Test]
        public void EditAsync_ThrowsException_WhenPetNotFound()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var model = new PetFormViewModel { Name = "NewName" };

            _petRepositoryMock
                .Setup(r => r.GetByIdAsync(petId))
                .ReturnsAsync((Pet?)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _service.EditAsync(petId.ToString(), model, "owner123"));
            _petRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Pet>()), Times.Never);
        }

        [Test]
        public async Task IsOwnerAsync_ReturnsTrue_WhenUserIsOwner()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var userId = "owner123";
            var pets = new List<Pet>
            {
                new Pet { Id = petId, OwnerId = userId, IsDeleted = false }
            };
            var mockSet = pets.BuildMock();
            _petRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.IsOwnerAsync(petId.ToString(), userId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsOwnerAsync_ReturnsFalse_WhenUserIsNotOwner()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var userId = "owner123";
            var otherUserId = "otherUser";
            var pets = new List<Pet>
            {
                new Pet { Id = petId, OwnerId = otherUserId, IsDeleted = false }
            };
            var mockSet = pets.BuildMock();
            _petRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(mockSet);

            // Act
            var result = await _service.IsOwnerAsync(petId.ToString(), userId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

