namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Favorites;

    public interface IFavoritesService
    {
        Task<IEnumerable<FavoritesViewModel>> GetUserFavoritesAsync(string userId);
    }
}
