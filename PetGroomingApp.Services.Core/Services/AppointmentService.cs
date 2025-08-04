namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Models.Enums;
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

        public async Task<string> CreateAsync(Appointment appointment)
        {
            await _appointmentRepository.AddAsync(appointment);
            return appointment.Id.ToString();
        }

        public async Task<bool> EditAsManagerAsync(string appointmentId, Appointment updated)
        {
            if (!Guid.TryParse(appointmentId, out var id))
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Cannot edit appointment.");

            appointment.AppointmentTime = updated.AppointmentTime;
            appointment.Duration = updated.Duration;
            appointment.Notes = updated.Notes;
            appointment.GroomerId = updated.GroomerId;
            appointment.PetId = updated.PetId;
            appointment.Status = updated.Status;
            appointment.UserId = updated.UserId;
            appointment.TotalPrice = updated.TotalPrice;
            appointment.AppointmentServices = updated.AppointmentServices;

            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<bool> EditAsync(string appointmentId, Appointment updated, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Cannot edit appointment.");
            if (appointment.UserId != userId)
                throw new UnauthorizedAccessException();

            appointment.AppointmentTime = updated.AppointmentTime;
            appointment.Duration = updated.Duration;
            appointment.Notes = updated.Notes;
            appointment.GroomerId = updated.GroomerId;
            appointment.PetId = updated.PetId;
            appointment.TotalPrice = updated.TotalPrice;
            appointment.AppointmentServices = updated.AppointmentServices;

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

        // Static helper for time overlap
        public static bool IsTimeOverlapping(DateTime existingStart, TimeSpan existingDuration, DateTime newStart, TimeSpan newDuration)
        {
            var existingEnd = existingStart.Add(existingDuration);
            var newEnd = newStart.Add(newDuration);
            return existingStart < newEnd && existingEnd > newStart;
        }
    }
}
