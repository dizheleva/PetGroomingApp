namespace PetGroomingApp.Web.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var pets = await _petService.GetAllAsync();

            return View(pets);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var pet = await _petService.GetByIdAsync(id);

                if (pet == null)
                {
                    return NotFound();
                }

                return View(pet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving the pet details: {ex.Message}");
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
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                await _petService.AddAsync(model, userId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while creating the pet: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await _petService.GetForEditByIdAsync(id);

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
                bool editSuccess = await _petService.EditAsync(id, model);
                if (!editSuccess)
                {
                    return NotFound();
                }

                return this.RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while editing the pet: {e.Message}");
                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var groomer = await _petService.GetByIdAsync(id);
                if (groomer == null)
                {
                    return NotFound();
                }
                return View(groomer);
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
                await _petService.SoftDeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while deleting the pet: {e.Message}");
                return this.RedirectToAction(nameof(Index));
            }

        }
    }
}
