namespace PetGroomingApp.Web.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BaseController : Controller
    {
        protected bool IsUserAuthenticated()
        {
            if (User == null || User.Identity == null)
            {
                return false;
            }

            return User.Identity.IsAuthenticated;
        }

        protected string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
