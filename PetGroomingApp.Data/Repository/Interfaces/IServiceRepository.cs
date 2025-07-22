namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IServiceRepository : IRepository<Service, Guid>
    {
        Task<Service> GetTopRatedServicesAsync(int count);
    }
}
