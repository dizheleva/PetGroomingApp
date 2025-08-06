namespace PetGroomingApp.Data.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class AppointmentRepository : BaseRepository<Appointment, Guid>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Service>?> GetAppointmentServicesByIds(List<Guid> servicesIds)
        {
            if (servicesIds == null || servicesIds.Count == 0)
            {
                return null;
            }

            var services = await _context.Services
                .Where(s => servicesIds.Contains(s.Id))
                .ToListAsync(); 

            return services;
        }
    }
}
