namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceService.GetAllAsync();

            return View(services);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _serviceService.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var service = await _serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _serviceService.GetForEditByIdAsync(id);

            if (model == null)  
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ServiceFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _serviceService.EditAsync(id, model);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var service = await _serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _serviceService.SoftDeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
