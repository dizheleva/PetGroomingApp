namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Pet;

    public interface IPetService
    {
        Task<IEnumerable<AllPetsIndexViewModel>> GetAllAsync();
        Task AddAsync(PetFormViewModel model, string userId);
        Task<PetDetailsViewModel?> GetByIdAsync(string? id);
        Task<PetFormViewModel?> GetForEditByIdAsync(string? id);
        Task<bool> EditAsync(string? id, PetFormViewModel? model);
        Task<bool> SoftDeleteAsync(string? id);
        Task<bool> HardDeleteAsync(string? id);
    }
}
