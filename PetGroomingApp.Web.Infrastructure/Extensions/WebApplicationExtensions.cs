namespace PetGroomingApp.Web.Infrastructure.Extensions
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;
    using PetGroomingApp.Data.Seeding.Interfaces;

    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UserAdminRedirection(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminRedirectionMiddleware>();

            return app;
        }

        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder app)
        {
            try
            {
                using IServiceScope scope = app.ApplicationServices.CreateScope();
                IServiceProvider serviceProvider = scope.ServiceProvider;

                IIdentitySeeder identitySeeder = serviceProvider
                    .GetRequiredService<IIdentitySeeder>();
                identitySeeder
                    .SeedIdentityAsync()
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                // Log error but don't block application startup
                Console.WriteLine($"Warning: Identity seeding failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return app;
        }
    }
}
