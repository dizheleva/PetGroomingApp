namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Favorites;

    public interface IFavoritesService
    {
        Task<IEnumerable<FavoritesViewModel>> GetUserFavoritesAsync(string userId);
        Task<bool> IsServiceInFavoritesAsync(string userId, Guid serviceId);
        Task AddToFavoritesAsync(string userId, string serviceId);
        Task RemoveFromFavoritesAsync(string userId, string serviceId);
    }
}
