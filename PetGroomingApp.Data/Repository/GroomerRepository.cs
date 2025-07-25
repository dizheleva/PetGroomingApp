namespace PetGroomingApp.Data.Repository
{
    using System.Threading.Tasks;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class GroomerRepository : BaseRepository<Groomer, Guid>, IGroomerRepository
    {
        public GroomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Groomer> GetTopRatedGroomersAsync(int count) => throw new NotImplementedException();
    }
}
