namespace PetGroomingApp.Services.Core.Interfaces
{
    using PetGroomingApp.Web.ViewModels.Appointment;

    public interface IAppointmentService 
    {
        Task<string> CreateAsync(AppointmentFormViewModel model, string? userId);
        Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string? userId);
        Task<AppointmentDetailsViewModel?> GetDetailsAsManagerAsync(string appointmentId);
        Task<AppointmentFormViewModel?> GetForEditByIdAsync(string appointmentId, string? userId);
        Task<bool> EditAsync(string appointmentId, AppointmentFormViewModel model, string? userId);
        Task<bool> CancelAsync(string appointmentId, string? userId);

        Task<IEnumerable<AppointmentListViewModel>> GetAllAsync(); // All appointments
        Task<IEnumerable<AppointmentListViewModel>> GetByUserAsync(string userId);
        Task<IEnumerable<AppointmentListViewModel>> GetByDateAsync(DateTime date);
        Task<bool> EditAsManagerAsync(string appointmentId, AppointmentFormViewModel model);

        Task<bool> IsOwnerAsync(string appointmentId, string userId);
        Task<bool> IsOverlappingAsync(Guid groomerId, DateTime startTime, TimeSpan duration, Guid? appointmentId = null);

    }
}
