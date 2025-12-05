namespace PetGroomingApp.Services.Core.Services
{
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Pet;

    using static PetGroomingApp.Services.Common.Constants.Pet;

    public class PetService : BaseService<Pet>, IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        : base(petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<string> CreateAsync(PetFormViewModel model, string? ownerId)
        {
            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Type = model.Type,
                Breed = model.Breed,
                Size = model.Size,
                Gender = model.Gender,
                Age = model.Age,
                ImageUrl = model.ImageUrl ?? DefaultPetUrl,
                Notes = model.Notes,
                OwnerId = ownerId
            };

            await _petRepository.AddAsync(pet);

            return pet.Id.ToString();
        }
                
        public async Task<PetDetailsViewModel> GetDetailsAsync(string? petId, string? ownerId)
        {
            var pet = await _petRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Id.ToString() == petId && p.OwnerId == ownerId && !p.IsDeleted)
                .Select(p => new PetDetailsViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    Size = p.Size,
                    Gender = p.Gender,
                    Age = p.Age,
                    Notes = p.Notes,
                    ImageUrl = p.ImageUrl
                })
                .FirstOrDefaultAsync();

            return pet ?? throw new InvalidOperationException(InvalidOperationMessage);
        }

        public async Task<PetFormViewModel> GetPetForEditAsync(string? petId, string? ownerId)
        {
            bool isGuidValid = Guid.TryParse(petId, out Guid petGuid);
            
            if (!isGuidValid)
            {
                throw new InvalidOperationException(IdInvalidMessage);
            }

            var pet = await _petRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Id == petGuid && !p.IsDeleted)
                .Select(p => new PetFormViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    Size = p.Size,
                    Gender = p.Gender,
                    Age = p.Age,
                    Notes = p.Notes,
                    ImageUrl = p.ImageUrl,
                    OwnerId = ownerId
                })
                .FirstOrDefaultAsync();

            return pet ?? throw new InvalidOperationException(PetNotFoundMessage);
        }

        public async Task<bool> EditAsync(string? petId, PetFormViewModel? model, string? ownerId)
        {
            bool isGuidValid = Guid.TryParse(petId, out Guid idGuid);

            if (!isGuidValid || model == null)
            {
                return false;
            }

            var pet = await _petRepository.GetByIdAsync(idGuid);
            
            if (pet == null || pet.IsDeleted)
            {
                throw new InvalidOperationException(NotEditableMessage);
            }

            if (pet.OwnerId != null && pet.OwnerId != ownerId)
            {
                throw new UnauthorizedAccessException();
            }

            pet.Name = model.Name;
            pet.Type = model.Type;
            pet.Breed = model.Breed;
            pet.Size = model.Size;
            pet.Gender = model.Gender;
            pet.Age = model.Age;
            pet.ImageUrl = model.ImageUrl ?? DefaultPetUrl;
            pet.Notes = model.Notes;

            return await _petRepository.UpdateAsync(pet);
        }

        public async Task<bool> IsOwnerAsync(string? petId, string? userId)
        {
            return await _petRepository
                .GetAllAttached()
                .AsNoTracking()
                .AnyAsync(p => p.Id.ToString() == petId && p.OwnerId == userId && !p.IsDeleted);
        }

        public async Task<IEnumerable<AllPetsViewModel?>> GetAllPetsAsync()
        {
            return await _petRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Select(p => new AllPetsViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Type = p.Type.ToString(),
                    Breed = p.Breed,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AllPetsViewModel?>> GetPetsByUserAsync(string? userId)
        {
            return await _petRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => p.OwnerId == userId && !p.IsDeleted)
                .Select(p => new AllPetsViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Type = p.Type.ToString(),
                    Breed = p.Breed,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }
    }
}