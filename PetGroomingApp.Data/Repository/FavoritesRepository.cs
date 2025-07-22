namespace PetGroomingApp.Data.Repository
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class FavoritesRepository : BaseRepository<UserService, string>, IFavoritesRepository
    {
        private readonly ApplicationDbContext _context;
        public FavoritesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(string userId, string serviceId)
        {
            return await this.GetAllAttached()
                .AnyAsync(us => us.UserId == userId && us.ServiceId.ToString() == serviceId);
        }
        public async Task<UserService?> GetByCompositeKeyAsynk(string userId, string serviceId)
        {
            return await this.GetAllAttached()
                .FirstOrDefaultAsync(us => us.UserId == userId && us.ServiceId.ToString() == serviceId);
        }
    }
}
