namespace PetGroomingApp.Data.Repository.Interfaces
{
    using PetGroomingApp.Data.Models;

    public interface IFavoritesRepository : IRepository<UserService, string>
    {
        Task<UserService?> GetByCompositeKeyAsynk(string userId, string serviceId);

        Task<bool> ExistsAsync(string userId, string serviceId);
    }
}
