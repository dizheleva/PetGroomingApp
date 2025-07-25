namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Groomer;

    public interface IGroomerService
    {
        Task<IEnumerable<AllGroomersIndexViewModel>> GetAllAsync();
        Task AddAsync(GroomerFormViewModel model);
        Task<GroomerDetailsViewModel?> GetByIdAsync(string? id);
        Task<GroomerFormViewModel?> GetForEditByIdAsync(string? id);
        Task<bool> EditAsync(string? id, GroomerFormViewModel? model);
        Task<bool> SoftDeleteAsync(string? id);
        Task<bool> HardDeleteAsync(string? id);
    }
}
