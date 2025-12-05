namespace PetGroomingApp.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;

    public class ServicesMenuViewComponent : ViewComponent
    {
        private readonly IServiceService _serviceService;

        public ServicesMenuViewComponent(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var services = await _serviceService.GetAllAsync();
            return View(services);
        }
    }
}

