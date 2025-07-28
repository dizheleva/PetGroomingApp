namespace PetGroomingApp.Data.Repository
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class AppointmentRepository : BaseRepository<Appointment, Guid>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
