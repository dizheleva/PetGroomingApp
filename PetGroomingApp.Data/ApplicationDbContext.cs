namespace PetGroomingApp.Data
{
    using System.Reflection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data.Models;

    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Pet> Pets { get; set; } = null!;
        public virtual DbSet<Groomer> Groomers { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<AppointmentService> AppointmentServices { get; set; } = null!;
        public virtual DbSet<UserService> UserServices { get; set; } = null!;
        public virtual DbSet<Manager> Managers { get; set; } = null!;
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Seeding a  'Administrator' role to AspNetRoles table
            //builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });


            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();


            //Seeding the User to AspNetUsers table
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", // primary key
                    UserName = "user1@mail.com",
                    NormalizedUserName = "USER1@MAIL.COM",
                    Email = "user1@mail.com",
                    NormalizedEmail = "USER1@MAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "123123")
                }
            );

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                    UserName = "user2@mail.com",
                    NormalizedUserName = "USER2@MAIL.COM",
                    Email = "user2@mail.com",
                    NormalizedEmail = "USER2@MAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "456456")
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            //builder.Entity<IdentityUserRole<string>>().HasData(
            //    new IdentityUserRole<string>
            //    {
            //        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
            //        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
            //    }
            //);
        }
    }
}
