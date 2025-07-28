namespace PetGroomingApp.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PetGroomingApp.Data.Repository;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public class AppointmentService : IAppointmentService
    { 
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Task AddAsync(AppointmentFormViewModel model) => throw new NotImplementedException();
        public Task<bool> EditAsync(string? id, AppointmentFormViewModel? model) => throw new NotImplementedException();
        public Task<IEnumerable<AllAppointmetIndexViewModel>> GetAllAsync() => throw new NotImplementedException();
        public Task<AppointmentFormViewModel?> GetByIdAsync(string? id) => throw new NotImplementedException();
        public Task<AppointmentFormViewModel?> GetForEditByIdAsync(string? id) => throw new NotImplementedException();
        public async Task<bool> HardDeleteAsync(string? id)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid idGuid);

            if (!isGuidValid)
            {
                return false;
            }

            var pet = await _appointmentRepository.GetByIdAsync(idGuid);

            if (pet == null)
            {
                return false;
            }

            return await _appointmentRepository.HardDeleteAsync(pet);
        }
        public async Task<bool> SoftDeleteAsync(string? id)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid idGuid);

            if (!isGuidValid)
            {
                return false;
            }

            var appointment = await _appointmentRepository.GetByIdAsync(idGuid);

            if (appointment == null)
            {
                return false;
            }

            return await _appointmentRepository.SoftDeleteAsync(appointment);
        }
    }
}
