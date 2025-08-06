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

    using static PetGroomingApp.Services.Common.Constants.Appointment;

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

        public async Task<string> CreateAsManagerAsync(AppointmentManagerFormViewModel model, string? id)
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

        public async Task<string> CreateAsync(AppointmentUserFormViewModel model, string? id)
        {
            if (model.SelectedServiceIds == null || !model.SelectedServiceIds.Any())
            {
                return string.Empty;
            }

            var selectedServices = await _appointmentRepository.GetAppointmentServicesByIds(model.SelectedServiceIds); ;

            if (selectedServices == null || !selectedServices.Any()) return string.Empty;

            var appointmentStatus = AppointmentStatus.Pending;
            if (Enum.TryParse<AppointmentStatus>(model.Status, true, out var statusEnum))
            {
                appointmentStatus = statusEnum;
            }

            var totalDuration = selectedServices.Sum(s => s.Duration.TotalMinutes);
            var totalPrice = selectedServices.Sum(s => s.Price);

            var appoinmentId = Guid.NewGuid();

            var appointment = new Appointment
            {
                Id = appoinmentId,
                AppointmentTime = model.AppointmentTime,
                Duration = TimeSpan.FromMinutes(totalDuration),
                Notes = model.Notes,
                GroomerId = model.SelectedGroomerId,
                UserId = id,
                Status = appointmentStatus,
                TotalPrice = totalPrice,
                AppointmentServices = model.SelectedServiceIds.Select(s => new Data.Models.AppointmentService
                {
                    AppointmentId = appoinmentId,
                    ServiceId = s
                }).ToList()
            };

            await _appointmentRepository.AddAsync(appointment);
            return appointment.Id.ToString();
        }

        public async Task<AppointmentUserFormViewModel?> GetForEditByIdAsync(string id, string userId)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid appointmentGuid);

            if (!isGuidValid)
            {
                throw new InvalidOperationException();
            }

            var appointment = await _appointmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(a => a.Id == appointmentGuid && a.Status != AppointmentStatus.Canceled)
                .Select(a => new AppointmentUserFormViewModel
                {
                    Id = a.Id,
                    AppointmentTime = a.AppointmentTime,
                    SelectedServiceIds = a.AppointmentServices.Select(s => s.ServiceId).ToList(),
                    Notes = a.Notes,
                    SelectedPetId = a.PetId,
                    SelectedGroomerId = a.GroomerId,
                    Status = a.Status.ToString(),
                    UserId = a.UserId ?? string.Empty,
                    TotalDuration = a.Duration,
                    TotalPrice = a.TotalPrice
                }).FirstOrDefaultAsync();

            return appointment;
        }

        public async Task<bool> EditAsManagerAsync(string appointmentId, AppointmentUserFormViewModel model)
        {
            if (!Guid.TryParse(appointmentId, out var id))
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException(NotEditableMessage);

            if (model.AppointmentTime < DateTime.UtcNow)
                throw new InvalidOperationException(NotPastTimeMessage);

            if (Enum.TryParse<AppointmentStatus>(model.Status, true, out var statusEnum))
            {
                appointment.Status = statusEnum;
            }
            else
            {
                appointment.Status = AppointmentStatus.Pending; // or throw exception
            }

            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Duration = model.TotalDuration;
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.SelectedGroomerId;
            appointment.PetId = model.SelectedPetId;
            appointment.UserId = model.UserId;
            appointment.TotalPrice = model.TotalPrice;
            UpdateAppointmentServices(appointment, model.SelectedServiceIds);

            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<bool> EditAsync(string appointmentId, AppointmentUserFormViewModel model, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Canceled)
                throw new InvalidOperationException(NotEditableMessage);

            if (model.AppointmentTime < DateTime.UtcNow)
                throw new InvalidOperationException(NotPastTimeMessage);

            if (appointment.UserId != userId)
                throw new UnauthorizedAccessException();

            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Duration = model.TotalDuration;
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.SelectedGroomerId;
            appointment.PetId = model.SelectedPetId;
            appointment.TotalPrice = model.TotalPrice;
            UpdateAppointmentServices(appointment, model.SelectedServiceIds);

            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var users = await _appointmentRepository
                .GetAllAttached()
                .Select(a => a.User)
                .Distinct()
                .ToListAsync();
            //TODO: Needs paging/filtering
            return users;
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
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
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
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
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
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
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
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return null;

            var appointmentServicesIds = await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id == id && a.UserId == userId)
                .Select(a => a.AppointmentServices.Select(s => s.ServiceId.ToString()))
                .ToListAsync();

            var appointmentServicesNames = await _appointmentRepository
                .GetAppointmentServicesNamesByIdsAsync(appointmentServicesIds.SelectMany(s => s).ToList());

            var appointment = await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id == id && a.UserId == userId)
                .Select(a => new AppointmentDetailsViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    Duration = (int)a.Duration.TotalMinutes,
                    Notes = a.Notes,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
                    Services = appointmentServicesNames,
                    Status = a.Status.ToString(),
                    OwnerName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : "",
                    Price = a.TotalPrice
                })
                .FirstOrDefaultAsync();

            return appointment;
        }

        public async Task<AppointmentDetailsViewModel?> GetDetailsAsManagerAsync(string appointmentId)
        {
            if (!Guid.TryParse(appointmentId, out var id))
                return null;

            var appointmentServicesIds = await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id == id)
                .Select(a => a.AppointmentServices.Select(s => s.ServiceId.ToString()))
                .ToListAsync();

            var appointmentServicesNames = await _appointmentRepository
                .GetAppointmentServicesNamesByIdsAsync(appointmentServicesIds.SelectMany(s => s).ToList());

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
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
                    Services = appointmentServicesNames,
                    Status = a.Status.ToString(),
                    OwnerName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : "",
                    Price = a.TotalPrice
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
                throw new InvalidOperationException(AlreadyCompletedMessage);

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

        public static void UpdateAppointmentServices(Appointment appointment, List<Guid> newServiceIds)
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
