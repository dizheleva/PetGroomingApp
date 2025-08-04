namespace PetGroomingApp.Services.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public class GroomerService : BaseService<Groomer>, IGroomerService
    {
        private readonly IGroomerRepository _groomerRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public GroomerService(IGroomerRepository groomerRepository, IAppointmentRepository appointmentRepository) : base(groomerRepository)
        {
            _groomerRepository = groomerRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<AllGroomersIndexViewModel>> GetAllAsync()
        {
            return await _groomerRepository.GetAllAttached()
                .AsNoTracking()
                .Where(g => !g.IsDeleted)
                .Select(g => new AllGroomersIndexViewModel
                {
                    Id = g.Id.ToString(),
                    Name = $"{g.FirstName} {g.LastName}",
                    JobTitle = g.JobTitle,
                    ImageUrl = g.ImageUrl
                })
                .ToListAsync();
        }

        public async Task AddAsync(GroomerFormViewModel model)
        {
            var groomer = new Groomer
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                JobTitle = model.JobTitle,
                ImageUrl = model.ImageUrl,
                Description = model.Description,
                PhoneNumber = model.PhoneNumber
            };

            await _groomerRepository.AddAsync(groomer);
        }

        public async Task<GroomerDetailsViewModel?> GetByIdAsync(string? id)
        {
            var groomer = await _groomerRepository.GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id.ToString() == id && !s.IsDeleted);

            if (groomer == null)
            {
                return null;
            }

            return new GroomerDetailsViewModel
            {
                Id = groomer.Id.ToString(),
                Name = $"{groomer.FirstName} {groomer.LastName}",
                JobTitle = groomer.JobTitle,
                ImageUrl = groomer.ImageUrl,
                PhoneNumber = groomer.PhoneNumber,
                Description = groomer.Description
            };
        }

        public async Task<GroomerFormViewModel?> GetForEditByIdAsync(string? id)
        {
            if (!Guid.TryParse(id, out Guid groomerGuid))
                return null;

            return await _groomerRepository.GetAllAttached()
                .Where(g => g.Id == groomerGuid && !g.IsDeleted)
                .Select(g => new GroomerFormViewModel
                {
                    Id = g.Id.ToString(),
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    JobTitle = g.JobTitle,
                    ImageUrl = g.ImageUrl,
                    Description = g.Description,
                    PhoneNumber = g.PhoneNumber,
                })
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EditAsync(string? id, GroomerFormViewModel? model)
        {
            if (model == null || !Guid.TryParse(id, out Guid groomerGuid))
                return false;

            var groomer = await _groomerRepository.GetByIdAsync(groomerGuid);
            if (groomer == null)
                return false;

            groomer.FirstName = model.FirstName;
            groomer.LastName = model.LastName;
            groomer.JobTitle = model.JobTitle;
            groomer.ImageUrl = model.ImageUrl;
            groomer.Description = model.Description;
            groomer.PhoneNumber = model.PhoneNumber;

            return await _groomerRepository.UpdateAsync(groomer);
        }

        public async Task<List<GroomerViewModel>> GetAvailableGroomersAsync(DateTime appointmentTime, int serviceDurationMinutes)
        {
            var allGroomers = await _groomerRepository.GetAllAsync();

            // Get all appointments on the same day with Confirmed status
            var appointments = await _appointmentRepository.GetAllAttached()
                .Where(a => a.AppointmentTime.Date == appointmentTime.Date &&
                            a.Status == Data.Models.Enums.AppointmentStatus.Confirmed)
                .ToListAsync();

            // Find groomers who are busy due to overlapping appointments
            var busyGroomerIds = appointments
                .Where(a => IsOverlapping(
                    a.AppointmentTime, (int)a.Duration.TotalMinutes,
                    appointmentTime, serviceDurationMinutes))
                .Select(a => a.GroomerId)
                .Distinct()
                .ToList();

            // Filter available groomers
            var available = allGroomers
                .Where(g => !busyGroomerIds.Contains(g.Id))
                .Select(g => new GroomerViewModel
                {
                    Id = g.Id,
                    Name = g.FirstName + " " + g.LastName,
                })
                .ToList();

            return available;
        }

        public async Task<List<DateTime>> GetAvailableTimesAsync(string groomerId, DateTime selectedDate)
        {
            var groomer = await _groomerRepository.GetAllAttached().FirstOrDefaultAsync(g => g.Id.ToString() == groomerId);
            List<DateTime> availableAppointmentTimes = new List<DateTime>();

            var busyAppointmentTimes = groomer?.Appointments
                .Where(a => a.AppointmentTime.Date == selectedDate.Date &&
                            a.Status == Data.Models.Enums.AppointmentStatus.Confirmed)
                .Select(a => a.AppointmentTime)
                .ToList() ?? new List<DateTime>();

            // Generate slots (every 30 minutes from 9:00 to 17:30)
            for (int hour = 9; hour <= 17; hour++)
            {
                var baseTime = selectedDate.Date.AddHours(hour);
                availableAppointmentTimes.Add(baseTime);
                availableAppointmentTimes.Add(baseTime.AddMinutes(30));
            }
            // Remove busy times
            availableAppointmentTimes = availableAppointmentTimes
                .Where(time => !busyAppointmentTimes.Contains(time))
                .ToList();

            return availableAppointmentTimes;
        }

        // Helpers:
        public static bool IsOverlapping(DateTime existingStart, int existingDuration, DateTime requestedStart, int requestedDuration)
        {
            var existingEnd = existingStart.AddMinutes(existingDuration);
            var requestedEnd = requestedStart.AddMinutes(requestedDuration);
            return existingStart < requestedEnd && existingEnd > requestedStart;
        }
    }
}
