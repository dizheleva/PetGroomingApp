namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Groomer;

    public class GroomerController : BaseController
    {
        private readonly IGroomerService _groomerService;
        private readonly ILogger<GroomerController> _logger;

        public GroomerController(IGroomerService groomerService, ILogger<GroomerController> logger)
        {
            _groomerService = groomerService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null)
        {
            const int pageSize = 6;
            var groomers = await _groomerService.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                groomers = groomers.Where(g => 
                    g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (g.JobTitle != null && g.JobTitle.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Pagination
            var totalItems = groomers.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedGroomers = Infrastructure.Helpers.PaginationHelper.Paginate(groomers, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;

            return View(paginatedGroomers);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var groomer = await _groomerService.GetByIdAsync(id);

                if (groomer == null)
                {
                    return NotFound();
                }

                return View(groomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving groomer details. ID: {GroomerId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving the groomer details.");
                return View(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(GroomerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _groomerService.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating groomer");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the groomer. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await _groomerService.GetForEditByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving groomer for editing. ID: {GroomerId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the groomer for editing.";
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, GroomerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool editSuccess = await _groomerService.EditAsync(id, model);
                if (!editSuccess)
                {
                    return NotFound();
                }

                return this.RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error editing groomer. ID: {GroomerId}", id);
                TempData["ErrorMessage"] = "An error occurred while editing the groomer. Please try again.";
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var groomer = await _groomerService.GetByIdAsync(id);
                if (groomer == null)
                {
                    return NotFound();
                }
                return View(groomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving groomer for deletion. ID: {GroomerId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the groomer for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _groomerService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting groomer. ID: {GroomerId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the groomer. Please try again.";
                return this.RedirectToAction(nameof(Index));
            }

        }
    }
}
