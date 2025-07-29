namespace PetGroomingApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data;
    using PetGroomingApp.Data.Repository;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Data.Seeding;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Services.Core.Services;
    using PetGroomingApp.Web.Infrastructure.Middlewares;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            builder.Services
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
            builder.Services.AddScoped<IGroomerRepository, GroomerRepository>();
            builder.Services.AddScoped<IPetRepository, PetRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            builder.Services.AddScoped<IServiceService, ServiceService>();
            builder.Services.AddScoped<IFavoritesService, FavoritesService>();
            builder.Services.AddScoped<IGroomerService, GroomerService>();
            builder.Services.AddScoped<IPetService, PetService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();

            builder.Services.AddControllersWithViews();

            WebApplication? app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<ManagerAccessMiddleware>();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Seed roles and manager user
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await RoleSeedier.SeedRolesAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            app.Run();
        }
    }
}
