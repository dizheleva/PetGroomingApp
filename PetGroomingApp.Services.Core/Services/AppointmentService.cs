namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Models.Enums;
    using PetGroomingApp.Data.Repository;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public class AppointmentService : BaseService<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<bool> CancelAsync(string appointmentId, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.UserId != userId)
                return false;

            if (appointment.Status is AppointmentStatus.Completed or AppointmentStatus.Canceled)
                return false;

            appointment.Status = AppointmentStatus.Canceled;
            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<string> CreateAsync(AppointmentFormViewModel model, string? id)
        {
            var appoinmentId = Guid.NewGuid();
            var appointment = new Appointment
            {
                Id = appoinmentId,
                AppointmentTime = model.AppointmentTime,
                Duration = TimeSpan.FromMinutes(model.TotalDuration),
                Notes = model.Notes,
                GroomerId = model.SelectedGroomerId,
                UserId = id,
                Status = AppointmentStatus.Pending,
                TotalPrice = model.TotalPrice, 
                AppointmentServices = model.SelectedServiceIds.Select(s => new Data.Models.AppointmentService
                {
                    AppointmentId = appoinmentId,
                    ServiceId = s
                }).ToList()
            };
            
            await _appointmentRepository.AddAsync(appointment);
            return appointment.Id.ToString();
        }

        public async Task<bool> EditAsManagerAsync(string appointmentId, AppointmentFormViewModel model)
        {
            if (!Guid.TryParse(appointmentId, out var id))
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Cannot edit appointment.");

            if (model.AppointmentTime < DateTime.UtcNow)
                throw new InvalidOperationException("Appointment time cannot be in the past.");

            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Duration = TimeSpan.FromMinutes(model.TotalDuration);
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.SelectedGroomerId;
            appointment.PetId = model.SelectedPetId;
            appointment.Status = model.Status;
            appointment.UserId = model.UserId;
            appointment.TotalPrice = model.TotalPrice;
            UpdateAppointmentServices(appointment, model.SelectedServiceIds);

            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<bool> EditAsync(string appointmentId, AppointmentFormViewModel model, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Canceled)
                throw new InvalidOperationException("Cannot edit appointment.");

            if (model.AppointmentTime < DateTime.UtcNow)
                throw new InvalidOperationException("Appointment time cannot be in the past.");
                       
            if (appointment.UserId != userId)
                throw new UnauthorizedAccessException();

            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Duration = TimeSpan.FromMinutes(model.TotalDuration);
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.SelectedGroomerId;
            appointment.PetId = model.SelectedPetId;
            appointment.TotalPrice = model.TotalPrice;
            UpdateAppointmentServices(appointment, model.SelectedServiceIds);

            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentListViewModel>> GetAllAsync()
        {
            return await _appointmentRepository
                .GetAllAttached()
                .OrderBy(a => a.AppointmentTime)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentListViewModel>> GetByDateAsync(DateTime date)
        {
            return await _appointmentRepository.GetAllAttached()
                .Where(a => a.AppointmentTime.Date == date.Date)
                .OrderBy(a => a.AppointmentTime)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentListViewModel>> GetByUserAsync(string userId)
        {
            return await _appointmentRepository.GetAllAttached()
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.AppointmentTime)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentListViewModel>> GetByGroomerAsync(Guid groomerId)
        {
            return await _appointmentRepository.GetAllAttached()
                .Where(a => a.GroomerId == groomerId)
                .OrderBy(a => a.AppointmentTime)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return null;
            return await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id == id && a.UserId == userId)
                .Select(a => new AppointmentDetailsViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    Duration = (int)a.Duration.TotalMinutes,
                    Notes = a.Notes,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    PetName = a.Pet != null ? a.Pet.Name : "No pet",
                    Services = a.AppointmentServices.Select(s => s.ServiceId.ToString()).ToList(),
                    Status = a.Status.ToString(),
                    OwnerName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : ""
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AppointmentDetailsViewModel?> GetDetailsAsManagerAsync(string appointmentId)
        {
            if (!Guid.TryParse(appointmentId, out var id))
                return null;
            return await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id == id)
                .Select(a => new AppointmentDetailsViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    Duration = (int)a.Duration.TotalMinutes,
                    Notes = a.Notes,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    PetName = a.Pet != null ? a.Pet.Name : "No pet",
                    Services = a.AppointmentServices.Select(s => s.ServiceId.ToString()).ToList(),
                    Status = a.Status.ToString(),
                    OwnerName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : ""
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsOwnerAsync(string appointmentId, string userId)
            => await _appointmentRepository.GetAllAttached().AnyAsync(a => a.Id.ToString() == appointmentId && a.UserId == userId);

        public async Task<bool> CompleteAsync(string appointmentId)
        {
            if (!Guid.TryParse(appointmentId, out var id)) return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) return false;

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Appointment is already completed.");

            appointment.Status = AppointmentStatus.Completed;
            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<bool> IsOverlappingAsync(Guid groomerId, DateTime startTime, TimeSpan duration, Guid? excludeAppointmentId = null)
        {
            var endTime = startTime.Add(duration);
            var appointments = await _appointmentRepository.GetAllAttached()
                .Where(a => a.GroomerId == groomerId && a.Status != AppointmentStatus.Canceled)
                .Where(a => !excludeAppointmentId.HasValue || a.Id != excludeAppointmentId.Value)
                .ToListAsync();

            return appointments.Any(a => IsTimeOverlapping(a.AppointmentTime, a.Duration, startTime, duration));
        }

        // Helpers:
        public static bool IsTimeOverlapping(DateTime existingStart, TimeSpan existingDuration, DateTime newStart, TimeSpan newDuration)
        {
            var existingEnd = existingStart.Add(existingDuration);
            var newEnd = newStart.Add(newDuration);
            return existingStart < newEnd && existingEnd > newStart;
        }

        public static  void UpdateAppointmentServices(Appointment appointment, List<Guid> newServiceIds)
        {
            var currentServiceIds = appointment.AppointmentServices.Select(x => x.ServiceId).ToList();
            
            var toAddGuids = newServiceIds.Except(currentServiceIds).ToList();
            var toRemoveGuids = currentServiceIds.Except(newServiceIds).ToList();
            
            foreach (var serviceId in toAddGuids)
            {
                appointment.AppointmentServices.Add(new Data.Models.AppointmentService
                {
                    AppointmentId = appointment.Id,
                    ServiceId = serviceId
                });
            }

            var servicesToRemove = appointment.AppointmentServices
                .Where(x => toRemoveGuids.Contains(x.ServiceId))
                .ToList();
            foreach (var service in servicesToRemove)
            {
                appointment.AppointmentServices.Remove(service);
            }
        }
    }
}
