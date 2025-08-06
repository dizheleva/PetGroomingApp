namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Data.Seeding.Dtos;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public class GroomerService : BaseService<Groomer>, IGroomerService
    {
        private readonly IGroomerRepository _groomerRepository;

        public GroomerService(IGroomerRepository groomerRepository) : base(groomerRepository)
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
                    Name = $"{g.FirstName} {g.LastName}",
                    JobTitle = g.JobTitle,
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
                JobTitle = model.JobTitle,
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
                Name = $"{groomer.FirstName} {groomer.LastName}",
                JobTitle = groomer.JobTitle,
                ImageUrl = groomer.ImageUrl,
                PhoneNumber = groomer.PhoneNumber,
                Description = groomer.Description
            };
        }

        public async Task<GroomerFormViewModel?> GetForEditByIdAsync(string? id)
        {
            if (!Guid.TryParse(id, out Guid groomerGuid))
                return null;

            return await _groomerRepository.GetAllAttached()
                .Where(g => g.Id == groomerGuid && !g.IsDeleted)
                .Select(g => new GroomerFormViewModel
                {
                    Id = g.Id.ToString(),
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    JobTitle = g.JobTitle,
                    ImageUrl = g.ImageUrl,
                    Description = g.Description,
                    PhoneNumber = g.PhoneNumber,
                })
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EditAsync(string? id, GroomerFormViewModel? model)
        {
            if (model == null || !Guid.TryParse(id, out Guid groomerGuid))
                return false;

            var groomer = await _groomerRepository.GetByIdAsync(groomerGuid);
            if (groomer == null)
                return false;

            groomer.FirstName = model.FirstName;
            groomer.LastName = model.LastName;
            groomer.JobTitle = model.JobTitle;
            groomer.ImageUrl = model.ImageUrl;
            groomer.Description = model.Description;
            groomer.PhoneNumber = model.PhoneNumber;

            return await _groomerRepository.UpdateAsync(groomer);
        }

        public async Task<List<GroomerDto>> GetAvailableGroomersAsync(DateTime appointmentTime, int durationMinutes)
        {
            //if (appointmentTime < DateTime.Now)
            //{
            //    throw new ArgumentException();
            //}

            var availableGroomers = await _groomerRepository.GetAllAvailableAtAsync(appointmentTime, durationMinutes);
            
            if (availableGroomers == null || !availableGroomers.Any())
            {
                return new List<GroomerDto>();
            }

            return availableGroomers
                .Select(g => new GroomerDto
                {
                    Id = g.Id,
                    Name = $"{g.FirstName} {g.LastName}"
                })
                .ToList() 
                ?? new List<GroomerDto>();
        }

        public async Task<List<DateTime>> GetAvailableTimesAsync(string groomerId, int duration)
        {
            if (string.IsNullOrWhiteSpace(groomerId))
            {
                throw new ArgumentException();
            }

            var groomer = await _groomerRepository.GetAllAttached().FirstOrDefaultAsync(g => g.Id.ToString() == groomerId);
            
            if (groomer == null)
            {
                throw new ArgumentException();
            }

            return await _groomerRepository.GetGroomerAvailableTimes(groomer.Id, duration) 
                ?? new List<DateTime>();
        }
    }
}
