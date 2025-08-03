namespace PetGroomingApp.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;
    using PetGroomingApp.Data.Seeding.Interfaces;

    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseManagerAccessRestriction(this IApplicationBuilder app)
        {
            app.UseMiddleware<ManagerAccessMiddleware>();

            return app;
        }

        public static IApplicationBuilder UserAdminRedirection(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminRedirectionMiddleware>();

            return app;
        }

        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            IIdentitySeeder identitySeeder = serviceProvider
                .GetRequiredService<IIdentitySeeder>();
            identitySeeder
                .SeedIdentityAsync()
                .GetAwaiter()
                .GetResult();

            return app;
        }
    }
}
