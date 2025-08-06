namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Seeding.Dtos;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public interface IAppointmentService 
    {
        Task<bool> CancelAsync(string appointmentId, string userId);
        Task<string> CreateAsManagerAsync(AppointmentManagerFormViewModel model, string? id);
        Task<string> CreateAsync(AppointmentUserFormViewModel model, string? id);
        Task<AppointmentUserFormViewModel?> GetForEditByIdAsync(string id, string userId);
        Task<bool> EditAsManagerAsync(string appointmentId, AppointmentUserFormViewModel model);
        Task<bool> EditAsync(string appointmentId, AppointmentUserFormViewModel model, string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<AppointmentListViewModel>> GetAllAsync();
        Task<IEnumerable<AppointmentListViewModel>> GetByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentListViewModel>> GetByUserAsync(string userId);
        Task<IEnumerable<AppointmentListViewModel>> GetByGroomerAsync(Guid groomerId);
        Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string userId);
        Task<AppointmentDetailsViewModel?> GetDetailsAsManagerAsync(string appointmentId);
        Task<bool> IsOwnerAsync(string appointmentId, string userId);
        Task<bool> CompleteAsync(string appointmentId);        
    }
}
