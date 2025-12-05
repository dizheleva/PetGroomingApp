namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServiceController : BaseController
    {
        private readonly IServiceService _serviceService;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceService serviceService, ILogger<ServiceController> logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null)
        {
            const int pageSize = 6;
            var services = await _serviceService.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                services = services.Where(s => 
                    s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (s.Description != null && s.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Pagination
            var totalItems = services.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedServices = Infrastructure.Helpers.PaginationHelper.Paginate(services, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedServices);
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
                _logger.LogError(ex, "Error retrieving service details. ID: {ServiceId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the service details.");
                return View(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _logger.LogError(ex, "Error creating service");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the service. Please try again.");
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
                _logger.LogError(ex, "Error retrieving service for editing. ID: {ServiceId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the service for editing.";
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _logger.LogError(e, "Error editing service. ID: {ServiceId}", id);
                TempData["ErrorMessage"] = "An error occurred while editing the service. Please try again.";
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
                _logger.LogError(ex, "Error retrieving service for deletion. ID: {ServiceId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the service for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _serviceService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting service. ID: {ServiceId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the service. Please try again.";
                return this.RedirectToAction(nameof(Index));
            }
            
        }
    }
}
