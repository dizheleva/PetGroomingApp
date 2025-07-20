namespace PetGroomingApp.Data
{
    using System.Reflection;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;

    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Groomer> Groomers { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentService> AppointmentServices { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
