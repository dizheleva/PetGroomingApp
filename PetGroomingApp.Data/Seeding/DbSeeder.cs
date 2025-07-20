namespace PetGroomingApp.Data.Seeding
{
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Models.Enums;
    using PetGroomingApp.Data.Seeding.Dtos;
    using PetGroomingApp.Data.Seeding.Utilities;

    public class DbSeeder
    {
        private readonly ApplicationDbContext context;

        public DbSeeder(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task SeedAsync()
        {
            await SeedPetsAsync();
            await SeedGroomersAsync();
            await SeedServicesAsync();
            //await SeedAppointmentsAsync();
            //await SeedAppointmentServicesAsync();
        }

        private async Task SeedGroomersAsync()
        {
            if (!context.Groomers.Any()) return;

            var groomers = JsonSeederHelper.LoadSeedData<GroomerInputModel>("groomers.json");
            
            foreach (var g in groomers)
            {
                context.Groomers.Add(new Groomer
                {
                    Id = Guid.Parse(g.Id),
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    PhoneNumber = g.PhoneNumber,
                    Description = g.Description
                });
            }

            await context.SaveChangesAsync();
        }

        private async Task SeedServicesAsync()
        {
            if (!context.Services.Any()) return;

            var services = JsonSeederHelper.LoadSeedData<ServiceInputModel>("services.json");

            foreach (var s in services)
            {
                context.Services.Add(new Service
                { 
                    Id= Guid.NewGuid(),
                    Name = s.Name,
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task SeedPetsAsync()
        {
            if (context.Pets.Any()) return;

            var pets = JsonSeederHelper.LoadSeedData<PetInputModel>("Seeding/Input/pets.json")!;

            foreach (var p in pets)
            {
                context.Pets.Add(new Pet
                {
                    Id = Guid.Parse(p.Id),
                    Name = p.Name,
                    Type = (PetType)p.Type,
                    Breed = p.Breed,
                    Size = (PetSize)p.Size,
                    Age = p.Age,
                    ImageUrl = p.ImageUrl,
                    Notes = p.Notes,
                    OwnerId = p.OwnerId
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task SeedAppointmentsAsync()
        {
            if (context.Appointments.Any()) return;
                        
            var appointments = JsonSeederHelper.LoadSeedData<AppointmentInputModel>("Seeding/Input/appointments.json")!;

            foreach (var a in appointments)
            {
                context.Appointments.Add(new Appointment
                {
                    Id = Guid.NewGuid(),
                    Date = a.Date,
                    Notes = a.Notes,
                    PetId = a.PetId,
                    UserId = a.UserId,
                    GroomerId = a.GroomerId,
                    Status = (AppointmentStatus)a.Status
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task SeedAppointmentServicesAsync()
        {
            if (context.AppointmentServices.Any()) return;

            var links = JsonSeederHelper.LoadSeedData<AppointmentServiceInputModel>("Seeding/Input/appointmentServices.json")!;

            foreach (var l in links)
            {
                context.AppointmentServices.Add(new AppointmentService
                {
                    AppointmentId = l.AppointmentId,
                    ServiceId = l.ServiceId
                });
            }

            await context.SaveChangesAsync();
        }
    }

}
