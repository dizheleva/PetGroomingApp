namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public interface IGroomerService : IService<Groomer>
    {
        Task<IEnumerable<AllGroomersIndexViewModel>> GetAllAsync();
        Task AddAsync(GroomerFormViewModel model);
        Task<GroomerDetailsViewModel?> GetByIdAsync(string? id);
        Task<GroomerFormViewModel?> GetForEditByIdAsync(string? id);
        Task<bool> EditAsync(string? id, GroomerFormViewModel? model);
    }
}
