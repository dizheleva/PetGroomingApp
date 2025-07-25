namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;

    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = this.GetUserId();

                if (!IsUserAuthenticated() || userId == null)
                {
                    return this.Forbid();
                }

                var favorites = await _favoritesService.GetUserFavoritesAsync(userId);
               
                return View(favorites);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while retrieving favorites: {e.Message}");
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string? serviceId)
        {
            try
            {
                var userId = GetUserId();

                if (!IsUserAuthenticated() || userId == null)
                {
                    return this.Forbid();
                }                                

                var isInFavorites = await _favoritesService.IsServiceInFavoritesAsync(userId, serviceId);

                if (!isInFavorites)
                {
                    await _favoritesService.AddToFavoritesAsync(userId, serviceId);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while adding the service to favorites: {e.Message}");
                return RedirectToAction(nameof(Index), "Favorites");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string? serviceId)
        {
            try
            {
                var userId = GetUserId();

                if (!IsUserAuthenticated() || userId == null)
                {
                    return this.Forbid();
                }
                
                var isInFavorites = _favoritesService.IsServiceInFavoritesAsync(userId, serviceId).Result;

                if (isInFavorites)
                {
                   var result = await _favoritesService.RemoveFromFavoritesAsync(userId, serviceId);

                    if (!result)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to remove the service from favorites.");
                        return RedirectToAction(nameof(Index));
                    }
                }

                return RedirectToAction(nameof(Index), "Favorites");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, $"An error occurred while removing the service from favorites: {e.Message}");
                return RedirectToAction(nameof(Index), "Favorites");
            }
        }
    }
}
