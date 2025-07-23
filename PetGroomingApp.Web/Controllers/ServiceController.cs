namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServiceController : BaseController
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var services = await _serviceService.GetAllAsync();

            return View(services);
        }
                
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var service = await _serviceService.GetByIdAsync(id);

                if (service == null)
                {
                    return NotFound();
                }

                return View(service);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the service details: {ex.Message}");
                return View(nameof(Index));
            }
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

            try
            {
                await _serviceService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the service: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await _serviceService.GetForEditByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the service for editing: {ex.Message}");

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ServiceFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool editSuccess = await _serviceService.EditAsync(id, model);
                if (!editSuccess)
                {
                    return NotFound();
                }

                return this.RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var service = await _serviceService.GetByIdAsync(id);
                if (service == null)
                {
                    return NotFound();
                }
                return View(service);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _serviceService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
            
        }
    }
}
