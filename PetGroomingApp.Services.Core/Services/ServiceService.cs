namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServiceService : IServiceInterface
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
    }
}
