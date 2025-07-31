namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Favorites;

    public interface IFavoritesService 
    {
        Task<IEnumerable<FavoritesViewModel>> GetUserFavoritesAsync(string userId);
        Task<bool> IsServiceInFavoritesAsync(string? userId, string? serviceId);
        Task AddToFavoritesAsync(string userId, string serviceId);
        Task<bool> RemoveFromFavoritesAsync(string? userId, string? serviceId);
    }
}
