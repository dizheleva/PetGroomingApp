namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;

    public class ServiceController : Controller
    {
        private readonly IServiceInterface _serviceService;

        public ServiceController(IServiceInterface serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceService.GetAllAsync();

            return View(services);
        }
    }
}
