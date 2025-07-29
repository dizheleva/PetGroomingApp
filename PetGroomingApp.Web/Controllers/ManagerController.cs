namespace PetGroomingApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ManagerController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
