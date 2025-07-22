namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Web.ViewModels.Favorites;

    public class FavoritesController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var favorites = new List<FavoritesViewModel>();

            return View(favorites);
        }

        [Authorize]
        public IActionResult Add()
        {

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Remove()
        {

            return RedirectToAction(nameof(Index));
        }
    }
}
