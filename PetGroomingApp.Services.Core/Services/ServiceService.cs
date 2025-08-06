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
    using PetGroomingApp.Web.ViewModels.Service;

    using static PetGroomingApp.Services.Common.Constants.Service;

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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

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
            if (!Guid.TryParse(id, out var guid))
                return null;

            var service = await _serviceRepository.GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == guid && !s.IsDeleted);

            if (service == null)
                return null;

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
            if (!Guid.TryParse(id, out var guid))
                return null;

            return await _serviceRepository.GetAllAttached()
                .Where(s => s.Id == guid && !s.IsDeleted)
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

        public async Task<bool> EditAsync(string? id, ServiceFormViewModel? model)
        {
            if (model == null || !Guid.TryParse(id, out var guid))
                return false;

            var service = await _serviceRepository.GetByIdAsync(guid);
            if (service == null)
                return false;

            service.Name = model.Name;
            service.ImageUrl = model.ImageUrl;
            service.Description = model.Description;
            service.Duration = ParseDuration(model.Duration);
            service.Price = model.Price;

            return await _serviceRepository.UpdateAsync(service);
        }

        private static TimeSpan ParseDuration(string duration)
        {
            if (TimeSpan.TryParse(duration, out var parsedDuration))
                return parsedDuration;

            throw new ArgumentException(DurationInvalidMessage);
        }

        public async Task<int> GetTotalDurationAsync(List<string> serviceIds)
        {
            if (serviceIds == null || serviceIds.Count == 0)
                return 0;

            var guids = serviceIds
                .Select(id => Guid.TryParse(id, out var guid) ? guid : (Guid?)null)
                .Where(guid => guid.HasValue)
                .Select(guid => guid.Value)
                .ToList();

            if (guids.Count == 0)
                return 0;

            var services = await _serviceRepository.GetAllAttached()
                .Where(s => guids.Contains(s.Id) && !s.IsDeleted)
                .ToListAsync();

            var totalDuration = services.Sum(s => s.Duration.TotalMinutes);

            return (int)totalDuration;
        }

        public async Task<decimal> GetTotalPriceAsync(List<string> serviceIds)
        {
            if (serviceIds == null || serviceIds.Count == 0)
                return 0m;

            var guids = serviceIds
               .Select(id => Guid.TryParse(id, out var guid) ? guid : (Guid?)null)
               .Where(guid => guid.HasValue)
               .Select(guid => guid.Value)
               .ToList();

            if (guids.Count == 0)
                return 0m;

            var services = await _serviceRepository.GetAllAttached()
                .Where(s => guids.Contains(s.Id) && !s.IsDeleted)
                .ToListAsync();

            var totalPrice = services.Sum(s => s.Price);

            return totalPrice;
        }

        public async Task<TotalsDto> CalculateTotalsAsync(List<string> serviceIds)
        {
            var totalDuration = await this.GetTotalDurationAsync(serviceIds);
            var totalPrice = await this.GetTotalPriceAsync(serviceIds);

            return new TotalsDto
            {
                TotalDuration = (int)totalDuration,
                TotalPrice = totalPrice
            };
        }
    }
}
