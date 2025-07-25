namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> IsServiceInFavoritesAsync(string? userId, string? serviceId)
        {
            if (serviceId != null && userId != null)
            {
                bool isServiceIdValid = Guid.TryParse(serviceId, out Guid serviceGuid);

                if (isServiceIdValid)
                {
                    return await this._favoritesRepository.GetAllAttached()
                        .AnyAsync(us => us.UserId.ToLower() == userId &&
                                        us.ServiceId.ToString() == serviceGuid.ToString());                    
                }
            }

            return false;
        }

        public async Task AddToFavoritesAsync(string userId, string serviceId)
        {            
            var userService = new UserService
            {
                UserId = userId,
                ServiceId = Guid.Parse(serviceId)
            };

            await _favoritesRepository.AddAsync(userService);
        }
        public async Task<bool> RemoveFromFavoritesAsync(string? userId, string? serviceId)
        {
            if (serviceId != null && userId != null)
            {
                bool isServiceIdValid = Guid.TryParse(serviceId, out Guid serviceGuid);
                if (isServiceIdValid)
                {
                    var userService = await this._favoritesRepository.GetAllAttached()
                        .Where(us => us.UserId.ToLower() == userId &&
                                     us.ServiceId.ToString() == serviceGuid.ToString())
                        .SingleOrDefaultAsync();

                    if (userService != null)
                    {
                        return await this._favoritesRepository.HardDeleteAsync(userService);
                    }
                }
            }
            
            return false;
        }
    }
}
