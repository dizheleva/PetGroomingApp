namespace PetGroomingApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public class GroomersController : BaseAdminController
    {
        private readonly IGroomerService _groomerService;
        private readonly ILogger<GroomersController> _logger;

        public GroomersController(IGroomerService groomerService, ILogger<GroomersController> logger)
        {
            _groomerService = groomerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null)
        {
            const int pageSize = 6;
            var allGroomers = await _groomerService.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                allGroomers = allGroomers.Where(g => 
                    g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    g.JobTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Pagination
            var totalItems = allGroomers.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedGroomers = Infrastructure.Helpers.PaginationHelper.Paginate(allGroomers, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedGroomers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var groomer = await _groomerService.GetByIdAsync(id);

                if (groomer == null)
                {
                    TempData["ErrorMessage"] = "Groomer not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(groomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving groomer details as admin. ID: {GroomerId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the groomer details.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

