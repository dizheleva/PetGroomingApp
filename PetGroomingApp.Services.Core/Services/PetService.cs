namespace PetGroomingApp.Services.Core.Services
{
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Pet;

    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task AddAsync(PetFormViewModel model, string userId)
        {
            var pet = new Pet
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Type = model.Type,
                Breed = model.Breed,
                Size = model.Size,
                Age = model.Age,
                Gender = model.Gender,
                OwnerId = userId.ToString(),
                Notes = model.Notes
            };

            await _petRepository.AddAsync(pet);
        }
        public async Task<bool> EditAsync(string? id, PetFormViewModel? model)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid idGuid);
            
            if (!isGuidValid || model == null)
            {
                return false;
            }

            var pet = await _petRepository.GetByIdAsync(idGuid);
            
            if (pet == null || pet.IsDeleted)
            {
                return false;
            }

            pet.Name = model.Name;
            pet.ImageUrl = model.ImageUrl;
            pet.Type = model.Type;
            pet.Breed = model.Breed;
            pet.Size = model.Size;
            pet.Age = model.Age;
            pet.Gender = model.Gender;
            pet.Notes = model.Notes;

            return await _petRepository.UpdateAsync(pet);
        }
        public async Task<PetDetailsViewModel?> GetByIdAsync(string? id)
        {
            return await _petRepository.GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Id.ToString() == id && !p.IsDeleted)
                .Select(p => new PetDetailsViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    ImageUrl = p.ImageUrl ?? "img/pet/defaultPet.png",
                    Type = p.Type,
                    Breed = p.Breed,
                    Size = p.Size,
                    Gender = p.Gender,
                    Age = p.Age,
                    Notes = p.Notes
                })
                .FirstOrDefaultAsync();
        }
        public async Task<PetFormViewModel?> GetForEditByIdAsync(string? id)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid idGuid);

            if (!isGuidValid)
            {
                return null;
            }

            return await _petRepository.GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Id == idGuid && !p.IsDeleted)
                .Select(p => new PetFormViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    ImageUrl = p.ImageUrl ?? "img/pet/defaultPet.png",
                    Type = p.Type,
                    Breed = p.Breed,
                    Size = p.Size,
                    Age = p.Age,
                    Gender = p.Gender,
                    Notes = p.Notes
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> HardDeleteAsync(string? id)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid idGuid);
            
            if (!isGuidValid)
            {
                return false;
            }
            
            var pet = await _petRepository.GetByIdAsync(idGuid);
            
            if (pet == null)
            {
                return false;
            }

            return await _petRepository.HardDeleteAsync(pet);
        }
        public async Task<bool> SoftDeleteAsync(string? id)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid idGuid);

            if (!isGuidValid)
            {
                return false;
            }

            var pet = await _petRepository.GetByIdAsync(idGuid);
            
            if (pet == null)
            {
                return false;
            }

            return await _petRepository.SoftDeleteAsync(pet);
        }
        public async Task<IEnumerable<AllPetsIndexViewModel>> GetAllAsync()
        {
            return await _petRepository.GetAllAttached()
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Select(p => new AllPetsIndexViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Type = p.Type.ToString(),
                    Breed = p.Breed,
                    ImageUrl = p.ImageUrl ?? "img/pet/defaultPet.png"

                })
                .ToListAsync();
        }
    }
}
