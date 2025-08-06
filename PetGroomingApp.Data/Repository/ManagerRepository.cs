namespace PetGroomingApp.Data.Repository
{
    using Interfaces;
    using Models;

    public class ManagerRepository : BaseRepository<Manager, Guid>, IManagerRepository
    {
        public ManagerRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
