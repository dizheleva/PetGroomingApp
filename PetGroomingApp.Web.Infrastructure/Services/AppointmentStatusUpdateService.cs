namespace PetGroomingApp.Web.Infrastructure.Services
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PetGroomingApp.Services.Core.Interfaces;

    public class AppointmentStatusUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentStatusUpdateService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5); // Check every 5 minutes

        public AppointmentStatusUpdateService(
            IServiceProvider serviceProvider,
            ILogger<AppointmentStatusUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AppointmentStatusUpdateService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateAppointmentStatusesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating appointment statuses.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("AppointmentStatusUpdateService is stopping.");
        }

        private async Task UpdateAppointmentStatusesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();

            try
            {
                var updatedCount = await appointmentService.UpdateExpiredAppointmentsStatusAsync();
                
                if (updatedCount > 0)
                {
                    _logger.LogInformation($"Updated {updatedCount} expired appointment(s) status.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating appointment statuses.");
            }
        }
    }
}

