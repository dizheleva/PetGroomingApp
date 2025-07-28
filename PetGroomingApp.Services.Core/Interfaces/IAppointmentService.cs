namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Appointment;

    public interface IAppointmentService 
    {
        Task<IEnumerable<AllAppointmetIndexViewModel>> GetAllAsync();
        Task AddAsync(AppointmentFormViewModel model);
        Task<AppointmentFormViewModel?> GetByIdAsync(string? id);
        Task<AppointmentFormViewModel?> GetForEditByIdAsync(string? id);
        Task<bool> EditAsync(string? id, AppointmentFormViewModel? model);
        Task<bool> SoftDeleteAsync(string? id);
        Task<bool> HardDeleteAsync(string? id);
    }
}
