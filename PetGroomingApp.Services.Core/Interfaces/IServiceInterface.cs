namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Service;

    public interface IServiceInterface
    {
        Task<IEnumerable<AllServicesIndexViewModel>> GetAllAsync();
        Task AddAsync(ServiceFormViewModel model);
    }
}
