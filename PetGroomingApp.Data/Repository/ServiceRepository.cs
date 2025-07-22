namespace PetGroomingApp.Data.Repository
{
    using System.Threading.Tasks;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class ServiceRepository : BaseRepository<Service, Guid>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Service> GetTopRatedServicesAsync(int count) => throw new NotImplementedException();
    }
}
