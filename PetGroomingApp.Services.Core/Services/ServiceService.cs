namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AllServicesIndexViewModel>> GetAllAsync()
        {
            return await _context.Services
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

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task<ServiceDetailsViewModel> GetByIdAsync(string id)
        {
            var service = await _context.Services
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

        public async Task<ServiceFormViewModel?> GetForEditByIdAsync(string id)
        {
            return await _context.Services
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
                .FirstOrDefaultAsync();
        }
        public async Task EditAsync(string id, ServiceFormViewModel model)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id.ToString() == id);

            if (service == null)
            {
                return;
            }

            service.Name = model.Name;
            service.ImageUrl = model.ImageUrl;
            service.Description = model.Description;
            service.Duration = model.Duration;
            service.Price = model.Price;

            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id.ToString() == id);

            if (service != null && !service.IsDeleted)
            {
                service.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
        public async Task HardDeleteAsync(string id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id.ToString() == id);

            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
