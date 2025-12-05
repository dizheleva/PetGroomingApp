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

            // Build notes: include pet name if provided but no pet selected
            var notes = model.Notes ?? string.Empty;
            if (!model.SelectedPetId.HasValue && !string.IsNullOrWhiteSpace(model.PetName))
            {
                notes = string.IsNullOrWhiteSpace(notes) 
                    ? $"Pet Name: {model.PetName}" 
                    : $"{notes}\nPet Name: {model.PetName}";
            }

            // Convert local time to UTC for storage (datetime-local sends local time)
            var appointmentTimeUtc = model.AppointmentTime;
            if (model.AppointmentTime.Kind == DateTimeKind.Unspecified)
            {
                // Assume it's local time and convert to UTC
                appointmentTimeUtc = DateTime.SpecifyKind(model.AppointmentTime, DateTimeKind.Local).ToUniversalTime();
            }
            else if (model.AppointmentTime.Kind == DateTimeKind.Local)
            {
                appointmentTimeUtc = model.AppointmentTime.ToUniversalTime();
            }

            var appointment = new Appointment
            {
                Id = appoinmentId,
                AppointmentTime = appointmentTimeUtc,
                Duration = TimeSpan.FromMinutes(totalDuration),
                Notes = notes,
                PetId = model.SelectedPetId,
                GroomerId = model.SelectedGroomerId,
                UserId = string.IsNullOrWhiteSpace(id) ? null : id,
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

            var appointmentData = await _appointmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(a => a.Id == appointmentGuid && a.Status != AppointmentStatus.Canceled)
                .Select(a => new
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

            if (appointmentData == null)
                return null;

            // Convert UTC to local time for display in datetime-local input
            var appointmentTime = appointmentData.AppointmentTime;
            if (appointmentTime.Kind == DateTimeKind.Utc)
            {
                appointmentTime = appointmentTime.ToLocalTime();
            }
            else if (appointmentTime.Kind == DateTimeKind.Unspecified)
            {
                // Assume it's UTC if unspecified
                appointmentTime = DateTime.SpecifyKind(appointmentTime, DateTimeKind.Utc).ToLocalTime();
            }

            return new AppointmentUserFormViewModel
            {
                Id = appointmentData.Id,
                AppointmentTime = appointmentTime,
                SelectedServiceIds = appointmentData.SelectedServiceIds,
                Notes = appointmentData.Notes,
                SelectedPetId = appointmentData.SelectedPetId,
                SelectedGroomerId = appointmentData.SelectedGroomerId,
                Status = appointmentData.Status,
                UserId = appointmentData.UserId,
                TotalDuration = appointmentData.TotalDuration,
                TotalPrice = appointmentData.TotalPrice
            };
        }

        public async Task<bool> EditAsync(string appointmentId, AppointmentUserFormViewModel model, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id) || userId == null)
                return false;

            // Load appointment with all navigation properties for proper update
            var appointment = await _appointmentRepository.GetAllAttached()
                .Include(a => a.AppointmentServices)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (appointment == null || appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Canceled)
                throw new InvalidOperationException(NotEditableMessage);

            // Convert local time to UTC for storage (datetime-local sends local time)
            var appointmentTimeUtc = model.AppointmentTime;
            if (model.AppointmentTime.Kind == DateTimeKind.Unspecified)
            {
                // Assume it's local time and convert to UTC
                appointmentTimeUtc = DateTime.SpecifyKind(model.AppointmentTime, DateTimeKind.Local).ToUniversalTime();
            }
            else if (model.AppointmentTime.Kind == DateTimeKind.Local)
            {
                appointmentTimeUtc = model.AppointmentTime.ToUniversalTime();
            }

            if (appointmentTimeUtc < DateTime.UtcNow)
                throw new InvalidOperationException(NotPastTimeMessage);

            if (appointment.UserId != userId)
                throw new UnauthorizedAccessException();

            // Recalculate duration and price from selected services
            if (model.SelectedServiceIds == null || !model.SelectedServiceIds.Any())
            {
                throw new InvalidOperationException("At least one service must be selected.");
            }

            var selectedServices = await _appointmentRepository.GetAppointmentServicesByIds(model.SelectedServiceIds);
            if (selectedServices == null || !selectedServices.Any())
            {
                throw new InvalidOperationException("Invalid services selected.");
            }

            var totalDuration = selectedServices.Sum(s => s.Duration.TotalMinutes);
            var totalPrice = selectedServices.Sum(s => s.Price);

            // Update appointment properties
            appointment.AppointmentTime = appointmentTimeUtc;
            appointment.Duration = TimeSpan.FromMinutes(totalDuration);
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.SelectedGroomerId;
            appointment.PetId = model.SelectedPetId;
            appointment.TotalPrice = totalPrice;
            
            // Update appointment services
            UpdateAppointmentServices(appointment, model.SelectedServiceIds);

            // Save changes - appointment is already tracked since we loaded it with GetAllAttached().Include()
            // Use SaveChangesAsync directly instead of UpdateAsync to avoid Attach() conflict
            await _appointmentRepository.SaveChangesAsync();
            
            return true;
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
                    OwnerName = a.User != null ? a.User.UserName ?? a.User.Email ?? "Unknown" : "Unknown",
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
                    OwnerName = a.User != null ? a.User.UserName ?? a.User.Email ?? "Unknown" : "Unknown",
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
                    OwnerName = a.User != null ? a.User.UserName ?? a.User.Email ?? "Unknown" : "Unknown",
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
                    OwnerName = a.User != null ? a.User.UserName ?? a.User.Email ?? "Unknown" : "Unknown",
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string userId)
        {
            if (!Guid.TryParse(appointmentId, out var id))
                return null;

            // Build query - if userId is empty, admin can see all appointments
            var query = _appointmentRepository.GetAllAttached().Where(a => a.Id == id);
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(a => a.UserId == userId);
            }

            var appointmentServicesIds = await query
                .Include(a => a.AppointmentServices)
                .Select(a => a.AppointmentServices.Select(s => s.ServiceId.ToString()))
                .ToListAsync();

            var appointmentServicesNames = await _appointmentRepository
                .GetAppointmentServicesNamesByIdsAsync(appointmentServicesIds.SelectMany(s => s).ToList());

            var appointmentQuery = _appointmentRepository.GetAllAttached().Where(a => a.Id == id);
            if (!string.IsNullOrEmpty(userId))
            {
                appointmentQuery = appointmentQuery.Where(a => a.UserId == userId);
            }

            var appointmentData = await appointmentQuery
                .Select(a => new
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    Duration = (int)a.Duration.TotalMinutes,
                    Notes = a.Notes,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    PetName = a.Pet != null ? a.Pet.Name : NotChosenPetName,
                    Status = a.Status.ToString(),
                    OwnerName = a.User != null ? $"{a.User.FirstName} {a.User.LastName}" : "",
                    Price = a.TotalPrice
                })
                .FirstOrDefaultAsync();

            if (appointmentData == null)
                return null;

            // Convert UTC to local time for display
            var appointmentTime = appointmentData.AppointmentTime;
            if (appointmentTime.Kind == DateTimeKind.Utc)
            {
                appointmentTime = appointmentTime.ToLocalTime();
            }
            else if (appointmentTime.Kind == DateTimeKind.Unspecified)
            {
                // Assume it's UTC if unspecified
                appointmentTime = DateTime.SpecifyKind(appointmentTime, DateTimeKind.Utc).ToLocalTime();
            }

            return new AppointmentDetailsViewModel
            {
                Id = appointmentData.Id,
                AppointmentTime = appointmentTime,
                Duration = appointmentData.Duration,
                Notes = appointmentData.Notes,
                GroomerName = appointmentData.GroomerName,
                PetName = appointmentData.PetName,
                Services = appointmentServicesNames,
                Status = appointmentData.Status,
                OwnerName = appointmentData.OwnerName,
                Price = appointmentData.Price
            };
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

        public async Task<int> UpdateExpiredAppointmentsStatusAsync()
        {
            var now = DateTime.UtcNow;
            var expiredAppointments = await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.AppointmentTime < now && 
                           a.Status != AppointmentStatus.Completed && 
                           a.Status != AppointmentStatus.Canceled)
                .ToListAsync();

            int updatedCount = 0;
            foreach (var appointment in expiredAppointments)
            {
                if (appointment.Status == AppointmentStatus.Pending)
                {
                    appointment.Status = AppointmentStatus.Canceled;
                }
                else if (appointment.Status == AppointmentStatus.Confirmed)
                {
                    appointment.Status = AppointmentStatus.Completed;
                }

                await _appointmentRepository.UpdateAsync(appointment);
                updatedCount++;
            }

            return updatedCount;
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
