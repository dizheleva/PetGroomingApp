namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Pet;

    public class PetController : BaseController
    {
        private readonly IPetService _petService;
                
        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Manager"))
            {
                var pets = await _petService.GetAllPetsAsync();
                return View(pets);
            }

            var userPets = await _petService.GetPetsByUserAsync(GetUserId());
            return View(userPets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var ownerId = User.IsInRole("Manager") ? null : GetUserId();

                var pet = await _petService.GetDetailsAsync(id, ownerId);

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
        public async Task<IActionResult> Create(PetFormViewModel model)
        {
            string? userId = GetUserId();

            if (!ModelState.IsValid || userId.IsNullOrEmpty())
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"  Error: {err.ErrorMessage}");
                    }
                }

                return View(model);
            }

            try
            {
                var ownerId = User.IsInRole("Manager") ? null : userId;
                var petId = await _petService.CreateAsync(model, ownerId);

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
                var ownerId = User.IsInRole("Manager") ? null : GetUserId();
                var model = await _petService.GetPetForEditAsync(id, ownerId);

                if (model == null)
                {
                    return NotFound();
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the pet for editing: {ex.Message}");

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, PetFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool success = User.IsInRole("Manager")
                    ? await _petService.EditAsManagerAsync(id, model)
                    : await _petService.EditAsync(id, model, GetUserId());

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
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the pet for deletion: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
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
