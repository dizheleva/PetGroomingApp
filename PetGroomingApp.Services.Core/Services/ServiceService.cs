namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data;
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
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price,
                })
                .ToListAsync();
        }
    }
}
