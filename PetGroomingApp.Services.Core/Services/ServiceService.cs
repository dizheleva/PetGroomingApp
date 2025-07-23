namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<AllServicesIndexViewModel>> GetAllAsync()
        {
            return await _serviceRepository.GetAllAttached()
                .Where(s => !s.IsDeleted)
                .AsNoTracking() // improves performance when the results will only be read, not updated
                .Select(s => new AllServicesIndexViewModel
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Description = s.Description
                })
                .ToListAsync();
        }

        public async Task AddAsync(ServiceFormViewModel model)
        {
            var service = new Service
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                Duration = model.Duration,
                Price = model.Price,
            };

            await _serviceRepository.AddAsync(service);
        }

        public async Task<ServiceDetailsViewModel?> GetByIdAsync(string? id)
        {
            var service = await _serviceRepository.GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.ToString() == id && !s.IsDeleted);

            if (service == null)
            {
                return null;    
            }

            return new ServiceDetailsViewModel
            {
                Id = service.Id.ToString(),
                Name = service.Name,
                ImageUrl = service.ImageUrl,
                Description = service.Description,
                Duration = service.Duration,
                Price = service.Price
            };
        }

        public async Task<ServiceFormViewModel?> GetForEditByIdAsync(string? id)
        {
            ServiceFormViewModel? service = null;

            bool isGuidValid = Guid.TryParse(id, out Guid serviceGuid);
            
            if (isGuidValid)
            {
                service = await _serviceRepository.GetAllAttached()
                .Where(s => s.Id.ToString() == id && !s.IsDeleted)
                .Select(s => new ServiceFormViewModel
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price
                })
                .SingleOrDefaultAsync();
            }
            
            return service;
        }
        public async Task<bool> EditAsync(string? id, ServiceFormViewModel? model)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid serviceGuid);
            Service? service = null;

            if (isGuidValid)
            {
                service = await _serviceRepository.GetByIdAsync(serviceGuid);
            }
                
            if (service == null || model == null)
            {
                return false;
            }

            service.Name = model.Name;
            service.ImageUrl = model.ImageUrl;
            service.Description = model.Description;
            service.Duration = model.Duration;
            service.Price = model.Price;

            return await _serviceRepository.UpdateAsync(service);
        }

        public async Task<bool> SoftDeleteAsync(string? id)
        {
            var service = await _serviceRepository.GetAllAttached()
                .FirstOrDefaultAsync(s => s.Id.ToString() == id);

            if (service == null)
            {
                return false;
            }

            return await _serviceRepository.SoftDeleteAsync(service);
        }
        public async Task<bool> HardDeleteAsync(string? id)
        {
            var service = await _serviceRepository.GetAllAttached()
                .FirstOrDefaultAsync(s => s.Id.ToString() == id);

            if (service == null)
            {
                return false;
            }

            return await _serviceRepository.HardDeleteAsync(service);
        }
    }
}
