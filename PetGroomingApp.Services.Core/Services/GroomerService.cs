namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public class GroomerService : IGroomerService
    {
        private readonly IGroomerRepository _groomerRepository;

        public GroomerService(IGroomerRepository groomerRepository)
        {
            _groomerRepository = groomerRepository;
        }

        public async Task<IEnumerable<AllGroomersIndexViewModel>> GetAllAsync()
        {
            return await _groomerRepository.GetAllAttached()
                .AsNoTracking()
                .Where(g => !g.IsDeleted)
                .Select(g => new AllGroomersIndexViewModel
                {
                    Id = g.Id.ToString(),
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    ImageUrl = g.ImageUrl                    
                })
                .ToListAsync();
        }

        public async Task AddAsync(GroomerFormViewModel model)
        {
            var groomer = new Groomer
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                PhoneNumber = model.PhoneNumber
            };

            await _groomerRepository.AddAsync(groomer);
        }

        public async Task<GroomerDetailsViewModel?> GetByIdAsync(string? id)
        {
            var groomer = await _groomerRepository.GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.ToString() == id && !s.IsDeleted);

            if (groomer == null)
            {
                return null;
            }

            return new GroomerDetailsViewModel
            {
                Id = groomer.Id.ToString(),
                FirstName = groomer.FirstName,
                LastName = groomer.LastName,
                ImageUrl = groomer.ImageUrl,
                Description = groomer.Description
            };
        }

        public async Task<GroomerFormViewModel?> GetForEditByIdAsync(string? id)
        {
            GroomerFormViewModel? groomer = null;

            bool isGuidValid = Guid.TryParse(id, out Guid groomerGuid);

            if (isGuidValid)
            {
                groomer = await _groomerRepository.GetAllAttached()
                .Where(g => g.Id.ToString() == id && !g.IsDeleted)
                .Select(g => new GroomerFormViewModel
                {
                    Id = g.Id.ToString(),
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    ImageUrl = g.ImageUrl,
                    Description = g.Description,
                    PhoneNumber = g.PhoneNumber,
                })
                .SingleOrDefaultAsync();
            }

            return groomer;
        }
        public async Task<bool> EditAsync(string? id, GroomerFormViewModel? model)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid groomerGuid);
            Groomer? groomer = null;

            if (isGuidValid)
            {
                groomer = await _groomerRepository.GetByIdAsync(groomerGuid);
            }

            if (groomer == null || model == null)
            {
                return false;
            }

            groomer.FirstName = model.FirstName;
            groomer.LastName = model.LastName;
            groomer.ImageUrl = model.ImageUrl;
            groomer.Description = model.Description;
            groomer.PhoneNumber = model.PhoneNumber;

            return await _groomerRepository.UpdateAsync(groomer);
        }

        public async Task<bool> SoftDeleteAsync(string? id)
        {
            var groomer = await _groomerRepository.GetAllAttached()
                .SingleOrDefaultAsync(s => s.Id.ToString() == id);

            if (groomer == null)
            {
                return false;
            }

            return await _groomerRepository.SoftDeleteAsync(groomer);
        }
        public async Task<bool> HardDeleteAsync(string? id)
        {
            var groomer = await _groomerRepository.GetAllAttached()
                .SingleOrDefaultAsync(s => s.Id.ToString() == id);

            if (groomer == null)
            {
                return false;
            }

            return await _groomerRepository.HardDeleteAsync(groomer);
        }
    }
}
