﻿namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Service;

    public interface IServiceService
    {
        Task<IEnumerable<AllServicesIndexViewModel>> GetAllAsync();
        Task AddAsync(ServiceFormViewModel model);
        Task<ServiceDetailsViewModel?> GetByIdAsync(string? id);
        Task<ServiceFormViewModel?> GetForEditByIdAsync(string? id);
        Task<bool> EditAsync(string? id, ServiceFormViewModel? model);
        Task<bool> SoftDeleteAsync(string? id);
        Task<bool> HardDeleteAsync(string? id);
    }
}
