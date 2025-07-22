namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Favorites;

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
            if (!IsUserAuthenticated())
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var userId = GetUserId();

            var favorites = await _favoritesService.GetUserFavoritesAsync(userId);

            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string serviceId)
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var userId = GetUserId();

            var isInFavorites = await _favoritesService.IsServiceInFavoritesAsync(userId, Guid.Parse(serviceId));

            if (!isInFavorites)
            {
                await _favoritesService.AddToFavoritesAsync(userId, serviceId);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(string serviceId)
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            var userId = GetUserId();
            var serviceGuid = Guid.Parse(serviceId);
            var isInFavorites = _favoritesService.IsServiceInFavoritesAsync(userId, serviceGuid).Result;

            if (!isInFavorites)
            {
                _favoritesService.RemoveFromFavoritesAsync(userId, serviceId).Wait();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
