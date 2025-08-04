namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Web.ViewModels.Service;

    public interface IServiceService : IService<Service>
    {
        Task<IEnumerable<AllServicesIndexViewModel>> GetAllAsync();
        Task AddAsync(ServiceFormViewModel model);
        Task<ServiceDetailsViewModel?> GetByIdAsync(string? id);
        Task<ServiceFormViewModel?> GetForEditByIdAsync(string? id);
        Task<bool> EditAsync(string? id, ServiceFormViewModel? model);
        Task<int> GetTotalDurationAsync(List<string> serviceIds);
        Task<decimal> GetTotalPriceAsync(List<string> serviceIds);
    }
}
