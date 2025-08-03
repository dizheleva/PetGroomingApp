namespace PetGroomingApp.Data.Repository
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class AppointmentServiceRepository : BaseRepository<AppointmentService, Guid>, IAppointmentServiceRepository
    {
        public AppointmentServiceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
