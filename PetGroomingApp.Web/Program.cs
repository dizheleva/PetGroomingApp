namespace PetGroomingApp.Web
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PetGroomingApp.Data;
    using PetGroomingApp.Data.Models;
    using PetGroomingApp.Data.Repository.Interfaces;
    using PetGroomingApp.Data.Seeding;
    using PetGroomingApp.Data.Seeding.Interfaces;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
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
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    ConfigureIdentity(builder.Configuration, options);
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRepositories(typeof(IServiceRepository).Assembly);
            builder.Services.AddUserDefinedServices(typeof(IServiceService).Assembly);

            builder.Services.AddTransient<IIdentitySeeder, IdentitySeeder>();

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

            app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseManagerAccessRestriction(); ;
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureIdentity(IConfigurationManager configurationManager, IdentityOptions options)
        {
            options.SignIn.RequireConfirmedEmail =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedEmail");
            options.SignIn.RequireConfirmedAccount =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedPhoneNumber =
                configurationManager.GetValue<bool>($"IdentityConfig:SignIn:RequireConfirmedPhoneNumber");

            options.Password.RequiredLength =
                configurationManager.GetValue<int>($"IdentityConfig:Password:RequiredLength");
            options.Password.RequireNonAlphanumeric =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireNonAlphanumeric");
            options.Password.RequireDigit =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireDigit");
            options.Password.RequireLowercase =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireLowercase");
            options.Password.RequireUppercase =
                configurationManager.GetValue<bool>($"IdentityConfig:Password:RequireUppercase");
            options.Password.RequiredUniqueChars =
                configurationManager.GetValue<int>($"IdentityConfig:Password:RequiredUniqueChars");
        }
    }
}
