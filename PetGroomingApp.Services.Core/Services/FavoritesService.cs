namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PetGroomingApp.Data;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Favorites;

    public class FavoritesService : IFavoritesService
    {
        private readonly ApplicationDbContext _context;

        public FavoritesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<FavoritesViewModel>> GetUserFavoritesAsync(string userId)
        {
            return Task.FromResult<IEnumerable<FavoritesViewModel>>(
                _context.UserServices
                    .Where(f => f.UserId == userId)
                    .Select(f => new FavoritesViewModel
                    {
                        ServiceId = f.Service.Id.ToString(),
                        Name = f.Service.Name,
                        ImageUrl = f.Service.ImageUrl,
                        Price = f.Service.Price
                    })
                    .ToList()
            );
        }
    }
}
