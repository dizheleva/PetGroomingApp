namespace PetGroomingApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Service;

    public class ServicesController : BaseAdminController
    {
        private readonly IServiceService _serviceService;
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(IServiceService serviceService, ILogger<ServicesController> logger)
        {
            _serviceService = serviceService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null)
        {
            const int pageSize = 6;
            var allServices = await _serviceService.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allServices = allServices.Where(s => 
                    s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (s.Description != null && s.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Pagination
            var totalItems = allServices.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedServices = Infrastructure.Helpers.PaginationHelper.Paginate(allServices, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedServices);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var service = await _serviceService.GetByIdAsync(id);

                if (service == null)
                {
                    TempData["ErrorMessage"] = "Service not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(service);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service details as admin. ID: {ServiceId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the service details.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

