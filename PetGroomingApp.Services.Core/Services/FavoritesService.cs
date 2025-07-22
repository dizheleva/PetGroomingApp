namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Favorites;

    public class FavoritesService : IFavoritesService
    {
        private readonly IFavoritesRepository _favoritesRepository;

        public FavoritesService(IFavoritesRepository favoritesRepository)
        {
            _favoritesRepository = favoritesRepository;
        }

        public async Task<IEnumerable<FavoritesViewModel>> GetUserFavoritesAsync(string userId)
        {
            return await _favoritesRepository.GetAllAttached()
                .Where(us => us.UserId == userId)
                .Select(us => new FavoritesViewModel
                {
                    ServiceId = us.Service.Id.ToString(),
                    Name = us.Service.Name,
                    ImageUrl = us.Service.ImageUrl,
                    Price = us.Service.Price
                }).ToListAsync();
        }

        public async Task<bool> IsServiceInFavoritesAsync(string userId, Guid serviceId)
        {
            return await _favoritesRepository.GetAllAttached()
                .AnyAsync(us => us.UserId == userId && us.ServiceId == serviceId);
        }

        public async Task AddToFavoritesAsync(string userId, string serviceId)
        {
            var userService = new UserService
            {
                UserId = userId,
                ServiceId = Guid.Parse(serviceId)
            };

            await _favoritesRepository.AddAsync(userService);
            await _favoritesRepository.SaveChangesAsync();
        }
        public async Task RemoveFromFavoritesAsync(string userId, string serviceId)
        {
            var userService = await _favoritesRepository.GetAllAttached()
                .FirstOrDefaultAsync(us => us.UserId == userId && us.ServiceId == Guid.Parse(serviceId));

            if (userService != null)
            {
                _favoritesRepository.Delete(userService);
                await _favoritesRepository.SaveChangesAsync();
            }
        }
    }
}
