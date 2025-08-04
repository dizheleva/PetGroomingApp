namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public interface IAppointmentService 
    {
        Task<bool> CancelAsync(string appointmentId, string userId);
        Task<string> CreateAsync(AppointmentFormViewModel model, string? id);
        Task<bool> EditAsManagerAsync(string appointmentId, AppointmentFormViewModel model);
        Task<bool> EditAsync(string appointmentId, AppointmentFormViewModel model, string userId);
        Task<IEnumerable<AppointmentListViewModel>> GetAllAsync();
        Task<IEnumerable<AppointmentListViewModel>> GetByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentListViewModel>> GetByUserAsync(string userId);
        Task<IEnumerable<AppointmentListViewModel>> GetByGroomerAsync(Guid groomerId);
        Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string userId);
        Task<AppointmentDetailsViewModel?> GetDetailsAsManagerAsync(string appointmentId);
        Task<bool> IsOwnerAsync(string appointmentId, string userId);
        Task<bool> CompleteAsync(string appointmentId);
        Task<bool> IsOverlappingAsync(Guid groomerId, DateTime startTime, TimeSpan duration, Guid? excludeAppointmentId = null);
    }
}
