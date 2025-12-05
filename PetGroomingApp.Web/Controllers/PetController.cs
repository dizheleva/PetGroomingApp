namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Antiforgery;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Pet;

    public class PetController : BaseController
    {
        private readonly IPetService _petService;
        private readonly ILogger<PetController> _logger;
                
        public PetController(IPetService petService, ILogger<PetController> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchTerm = null, string? typeFilter = null)
        {
            const int pageSize = 6;
            IEnumerable<AllPetsViewModel?> pets = await _petService.GetPetsByUserAsync(GetUserId());

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                pets = pets.Where(p => p != null && (
                    p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Breed.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Apply type filter
            if (!string.IsNullOrWhiteSpace(typeFilter))
            {
                pets = pets.Where(p => p != null && p.Type.Equals(typeFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Pagination
            var totalItems = pets.Count();
            var pagination = Infrastructure.Helpers.PaginationHelper.CreatePagination(page, totalItems, pageSize);
            var paginatedPets = Infrastructure.Helpers.PaginationHelper.Paginate(pets, page, pageSize);

            ViewBag.Pagination = pagination;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.TypeFilter = typeFilter;

            return View(paginatedPets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var pet = await _petService.GetDetailsAsync(id, GetUserId());

                if (pet == null)
                {
                    return NotFound();
                }

                return View(pet);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
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
        public async Task<IActionResult> Create(PetFormViewModel model)
        {
            string? userId = GetUserId();

            if (!ModelState.IsValid || userId.IsNullOrEmpty())
            {
                _logger.LogWarning("Model validation failed for pet creation or user ID is empty");
                return View(model);
            }

            try
            {
                var petId = await _petService.CreateAsync(model, userId);

                TempData["Success"] = "Pet created successfully.";
                return RedirectToAction(nameof(Details), new { id = petId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await _petService.GetPetForEditAsync(id, GetUserId());

                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet for editing. ID: {PetId}", id);
                TempData["Error"] = "An error occurred while retrieving the pet for editing.";
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PetFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool success = await _petService.EditAsync(id, model, GetUserId());

                if (!success)
                {
                    TempData["Error"] = "Failed to edit pet.";
                    return NotFound();
                }

                TempData["Success"] = "Pet updated successfully.";
                return this.RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var pet = await _petService.GetDetailsAsync(id, GetUserId());
                if (pet == null)
                {
                    return NotFound();
                }

                return View(pet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet for deletion. ID: {PetId}", id);
                TempData["Error"] = "An error occurred while retrieving the pet for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                bool success = await _petService.SoftDeleteAsync(id);

                if (!success)
                {
                    TempData["Error"] = "Failed to delete pet.";
                }
                else
                {
                    TempData["Success"] = "Pet deleted successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
