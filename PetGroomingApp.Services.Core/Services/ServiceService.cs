namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    using static PetGroomingApp.Services.Common.EntityConstants.Service;

    public class ServiceService : BaseService<Service>, IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository) : base(serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<AllServicesIndexViewModel>> GetAllAsync()
        {
            return await _serviceRepository.GetAllAttached()
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
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
                Duration = ParseDuration(model.Duration),
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
                Duration = service.Duration.ToString(DurationFormat),
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
                    Duration = s.Duration.ToString(DurationFormat),
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
            service.Duration = ParseDuration(model.Duration);
            service.Price = model.Price;

            return await _serviceRepository.UpdateAsync(service);
        }

        private static TimeSpan ParseDuration(string duration)
        {
            if (TimeSpan.TryParse(duration, out TimeSpan parsedDuration))
            {
                return parsedDuration;
            }

            throw new ArgumentException(DurationInvalidMessage);
        }

        public Task<int> GetTotalDurationAsync(List<string> serviceIds) => throw new NotImplementedException();
        public Task<decimal> GetTotalPriceAsync(List<string> serviceIds) => throw new NotImplementedException();
    }
}
