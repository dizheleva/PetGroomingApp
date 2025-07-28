namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IAppointmentRepository : IRepository<Appointment, Guid>
    {
    }
}
