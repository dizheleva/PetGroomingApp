namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Web.ViewModels.Pet;

    public interface IPetService : IService<Pet>
    {
        Task<string> CreateAsync(PetFormViewModel model, string? ownerId);
        Task<PetFormViewModel> GetPetForEditAsync(string? petId, string? ownerId);
        Task<bool> EditAsync(string? petId, PetFormViewModel? model, string? ownerId);
        Task<PetDetailsViewModel> GetDetailsAsync(string? petId, string? ownerId);
        Task<bool> IsOwnerAsync(string? petId, string? userId);
        Task<IEnumerable<AllPetsViewModel?>> GetAllPetsAsync(); 
        Task<IEnumerable<AllPetsViewModel?>> GetPetsByUserAsync(string? userId);
        Task<bool> EditAsManagerAsync(string? petId, PetFormViewModel? model);
    }

}
