namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IAppointmentRepository : IRepository<Appointment, Guid>
    {
        Task<List<Service>?> GetAppointmentServicesByIds(List<Guid> servicesIds);
        Task<List<string>> GetAppointmentServicesNamesByIdsAsync(List<string> serviceIds);
    }
}
