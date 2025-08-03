namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Models.Enums;
    using PetGroomingApp.Data.Repository;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public class AppointmentService : BaseService<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IGroomerRepository _groomerRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository, IServiceRepository serviceRepository, IGroomerRepository groomerRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _groomerRepository = groomerRepository;
        }

        public async Task<bool> CancelAsync(string appointmentId, string? userId)
        {
            if (!Guid.TryParse(appointmentId, out Guid id) || userId == null)
            {
                return false;
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null || appointment.UserId != userId)
            {
                return false;
            }

            if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Canceled)
            {
                return false;
            }

            appointment.Status = AppointmentStatus.Canceled;

            return await _appointmentRepository.UpdateAsync(appointment);
        }
        public async Task<string> CreateAsync(AppointmentFormViewModel model, string? userId)
        {            
            var selectedServices = await _serviceRepository.GetAllAttached()
                .Where(s => model.SelectedServiceIds.Contains(s.Id))
                .ToListAsync();           

            var totalDuration = selectedServices.Sum(s => s.Duration.TotalMinutes); 
            var totalPrice = selectedServices.Sum(s => s.Price);
            var appointmentId = Guid.NewGuid();
            
            var appointment = new Appointment
            {
                Id = appointmentId,
                AppointmentTime = model.AppointmentTime,
                Duration = TimeSpan.FromMinutes(totalDuration),
                Notes = model.Notes,
                GroomerId = model.GroomerId,
                PetId = model.PetId,
                UserId = userId,
                Status = AppointmentStatus.Pending,
                TotalPrice = totalPrice,
                AppointmentServices = model.SelectedServiceIds != null
                    ? model.SelectedServiceIds.ConvertAll(serviceId => new Data.Models.AppointmentService
                    {
                        AppointmentId = appointmentId,
                        ServiceId = serviceId
                    }).ToList()
                    : []
            };

            await _appointmentRepository.AddAsync(appointment);
            return appointment.Id.ToString();
        }
        public async Task<bool> EditAsManagerAsync(string appointmentId, AppointmentFormViewModel model)
        {
            bool isGuidValid = Guid.TryParse(appointmentId, out Guid idGuid);

            if (!isGuidValid || model == null)
            {
                return false;
            }

            var appointment = await _appointmentRepository.GetByIdAsync(idGuid);

            if (appointment == null || appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot edit appointment.");
            }

            var selectedServices = await _serviceRepository.GetAllAttached()
                .Where(s => model.SelectedServiceIds.Contains(s.Id))
                .ToListAsync();

            var totalDuration = selectedServices.Sum(s => s.Duration.TotalMinutes);
            var totalPrice = selectedServices.Sum(s => s.Price);

            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Duration = TimeSpan.FromMinutes(totalDuration);
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.GroomerId;
            appointment.PetId = model.PetId;
            appointment.Status = model.Status;
            appointment.UserId = model.UserId;
            appointment.TotalPrice = totalPrice;
            appointment.AppointmentServices = model.SelectedServiceIds != null
                ? model.SelectedServiceIds.ConvertAll(serviceId => new Data.Models.AppointmentService
                {
                    AppointmentId = idGuid,
                    ServiceId = serviceId
                }).ToList()
                : [];
            
            return await _appointmentRepository.UpdateAsync(appointment);
        }
        public async Task<bool> EditAsync(string appointmentId, AppointmentFormViewModel model, string? userId)
        {
            bool isGuidValid = Guid.TryParse(appointmentId, out Guid idGuid);

            if (!isGuidValid || model == null || userId == null)
            {
                return false;
            }

            var appointment = await _appointmentRepository.GetByIdAsync(idGuid);

            if (appointment == null || appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot edit appointment.");
            }

            if (appointment.UserId != null && appointment.UserId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            var selectedServices = await _serviceRepository.GetAllAttached()
                .Where(s => model.SelectedServiceIds.Contains(s.Id))
                .ToListAsync();

            var totalDuration = selectedServices.Sum(s => s.Duration.TotalMinutes);
            var totalPrice = selectedServices.Sum(s => s.Price);

            appointment.AppointmentTime = model.AppointmentTime;
            appointment.Duration = TimeSpan.FromMinutes(totalDuration);
            appointment.Notes = model.Notes;
            appointment.GroomerId = model.GroomerId;
            appointment.PetId = model.PetId;
            appointment.TotalPrice = totalPrice;
            appointment.AppointmentServices = model.SelectedServiceIds != null
                ? model.SelectedServiceIds.ConvertAll(serviceId => new Data.Models.AppointmentService
                {
                    AppointmentId = idGuid,
                    ServiceId = serviceId
                }).ToList()
                : [];

            return await _appointmentRepository.UpdateAsync(appointment);
        }
        public async Task<IEnumerable<AppointmentListViewModel>> GetAllAsync()
        {
            return await _appointmentRepository
                .GetAllAttached()
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }
        public async Task<IEnumerable<AppointmentListViewModel>> GetByDateAsync(DateTime date)
        {
            return await _appointmentRepository.GetAllAttached()
                .Where(a => a.AppointmentTime == date)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }
        public async Task<IEnumerable<AppointmentListViewModel>> GetByUserAsync(string userId)
        {
            return await _appointmentRepository.GetAllAttached()
                .Where(a => a.UserId == userId)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentListViewModel>> GetByGroomerAsync(string groomerId)
        {
            return await _appointmentRepository.GetAllAttached()
                .Where(a => a.GroomerId.ToString() == groomerId)
                .Select(a => new AppointmentListViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    GroomerName = a.Groomer != null ? $"{a.Groomer.FirstName} {a.Groomer.LastName}" : "Not selected",
                    Status = a.Status.ToString()
                })
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();
        }
        public async Task<AppointmentDetailsViewModel?> GetDetailsAsync(string appointmentId, string? userId)
        {
            if (!Guid.TryParse(appointmentId, out Guid appointmentGuid) || userId == null)
            {
                return null;
            }

            return await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id.ToString() == appointmentId && a.UserId == userId)
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
                    OwnerName = $"{a.User.FirstName} {a.User.LastName}"
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AppointmentDetailsViewModel?> GetDetailsAsManagerAsync(string appointmentId)
        {
            return await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.Id.ToString() == appointmentId)
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
                    OwnerName = a.UserId == null ? "No owner" : $"{a.User.FirstName} {a.User.LastName}"
                })
                .FirstOrDefaultAsync();
        }
        public async Task<AppointmentFormViewModel?> GetForEditByIdAsync(string appointmentId, string? userId)
        {
            bool isGuidValid = Guid.TryParse(appointmentId, out Guid appointmentGuid);

            if (!isGuidValid)
            {
                throw new InvalidOperationException("Invalid ID.");
            }

            var appointment = await _appointmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(a => a.Id == appointmentGuid && a.Status != AppointmentStatus.Completed)
                .Select(a => new AppointmentFormViewModel
                {
                    Id = a.Id.ToString(),
                    AppointmentTime = a.AppointmentTime,
                    TotalDuration = (int)a.Duration.TotalMinutes,
                    Notes = a.Notes,
                    GroomerId = a.GroomerId,
                    PetId = a.PetId,
                    UserId = userId ?? null,
                    Status = a.Status,
                    SelectedServiceIds = a.AppointmentServices.Select(s => s.ServiceId).ToList(),
                    TotalPrice = a.TotalPrice
                })
                .FirstOrDefaultAsync();

            return appointment ?? throw new InvalidOperationException("Appointment not found.");
        }

        public async Task<bool> IsOwnerAsync(string appointmentId, string userId)
        {
            return await _appointmentRepository
                .GetAllAttached()
                .AsNoTracking()
                .AnyAsync(a => a.Id.ToString() == appointmentId && a.UserId == userId);
        }

        public async Task<bool> CompleteAsync(string appointmentId)
        {
            if (!Guid.TryParse(appointmentId, out Guid id)) return false;

            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null) return false;

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Appointment is already completed.");
            }

            appointment.Status = AppointmentStatus.Completed;
            return await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<bool> IsOverlappingAsync(Guid groomerId, DateTime startTime, TimeSpan duration, Guid? appointmentId = null)
        {
            var endTime = startTime.Add(duration);

            var appointments = await _appointmentRepository.GetAllAttached()
                .Where(a => a.GroomerId == groomerId && a.Status != AppointmentStatus.Canceled)
                .Where(a => appointmentId == null || a.Id != appointmentId)
                .ToListAsync();

            if (appointments == null)
            {
                return false;
            }

            foreach (var appointment in appointments)
            {
                if (IsTimeOverlapping(appointment, startTime, endTime))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<GroomerDto>> GetAvailableGroomersAsync(DateTime appointmentTime)
        {
            var allGroomers = await _groomerRepository.GetAllAsync();
            var busyGroomers = await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.AppointmentTime == appointmentTime)
                .Select(a => a.GroomerId)
                .ToListAsync();

            var available = allGroomers.Where(g => !busyGroomers.Contains(g.Id))
                .Select(g => new GroomerDto
                { 
                    Id = g.Id, 
                    Name = g.FirstName + " " + g.LastName,
                })
                .ToList();

            return available;
        }

        public async Task<DateTime> GetNextFreeTimeForGroomerAsync(Guid groomerId)
        {
            var appointments = await _appointmentRepository
                .GetAllAttached()
                .Where(a => a.GroomerId == groomerId && a.AppointmentTime > DateTime.Now)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            DateTime now = DateTime.Now;
            TimeSpan slot = TimeSpan.FromMinutes(30);

            for (int i = 0; i < 100; i++)
            {
                var proposed = now.AddMinutes(i * 30);
                bool conflict = appointments.Any(a => Math.Abs((a.AppointmentTime - proposed).TotalMinutes) < a.Duration.TotalMinutes);
                if (!conflict)
                    return proposed;
            }

            return now.AddDays(1);
        }

        public async Task<AppointmentCalculationDto> CalculateTotalAsync(List<Guid> serviceIds)
        {
            var services = await _serviceRepository.GetAllAttached()
                .Where(s => serviceIds.Contains(s.Id))
                .ToListAsync();

            var total = new AppointmentCalculationDto
            {
                TotalDuration = (int)services.Sum(s => s.Duration.TotalMinutes),
                TotalPrice = services.Sum(s => s.Price)
            };

            return total;
        }

        // Helper method to check if two time intervals overlap
        private static bool IsTimeOverlapping(Appointment appointment, DateTime startTime, DateTime endTime)
        {
            var existingStart = appointment.AppointmentTime;
            var existingEnd = appointment.AppointmentTime.Add(appointment.Duration);

            return startTime < existingEnd && existingStart < endTime;
        }

    }
}
