namespace PetGroomingApp.Data.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;

    public class GroomerRepository : BaseRepository<Groomer, Guid>, IGroomerRepository
    {
        private readonly ApplicationDbContext _context;
        public GroomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetGroomerAvailableTimes(Guid groomerId, int durationMinutes)
        {
            var now = DateTime.Now;
            var maxDate = now.Date.AddDays(30);
            var workDayStart = new TimeSpan(9, 0, 0);
            var workDayEnd = new TimeSpan(18, 0, 0);

            var appointments = await _context.Appointments
                .Where(a => a.GroomerId == groomerId && a.AppointmentTime >= now && a.AppointmentTime <= maxDate)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            var availableSlots = new List<DateTime>();

            DateTime current = RoundUpToNext30Minutes(now);

            while (current < maxDate)
            {
                if (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday)
                {
                    current = current.Date.AddDays(1).Add(workDayStart);
                    continue;
                }

                if (current.TimeOfDay < workDayStart)
                {
                    current = current.Date.Add(workDayStart);
                    continue;
                }

                if (current.AddMinutes(durationMinutes).TimeOfDay > workDayEnd)
                {
                    current = current.Date.AddDays(1).Add(workDayStart);
                    continue;
                }

                bool overlaps = appointments.Any(a =>
                    current < a.AppointmentTime.Add(a.Duration /* or a.Duration ?? TimeSpan.Zero */) &&
                    current.AddMinutes(durationMinutes) > a.AppointmentTime
                );

                if (!overlaps)
                {
                    availableSlots.Add(current);
                }

                current = current.AddMinutes(30);
            }

            return availableSlots;
        }

        private static DateTime RoundUpToNext30Minutes(DateTime dt)
        {
            int minutes = dt.Minute;
            int delta = 30 - (minutes % 30);
            if (delta == 30) delta = 0;
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0).AddMinutes(minutes + delta);
        }


        public async Task<List<Groomer>?> GetAllAvailableAtAsync(DateTime startTime, int durationMinutes)
        {
            if (durationMinutes <= 0)
            {
                return await _context.Groomers.ToListAsync();
            }

            var endTime = startTime.AddMinutes(durationMinutes);

            var relevantAppointments = await _context.Appointments
                .Where(a => a.AppointmentTime < endTime.AddMinutes(60)) // small buffer
                .ToListAsync();

            var overlappingGroomerIds = relevantAppointments
                .Where(a => a.AppointmentTime <= endTime &&
                            a.AppointmentTime.Add(a.Duration) > startTime)
                .Select(a => a.GroomerId)
                .Distinct()
                .ToList();

            var availableGroomers = await _context.Groomers
                .Where(g => !overlappingGroomerIds.Contains(g.Id))
                .ToListAsync();

            return availableGroomers;
        }

    }
}
