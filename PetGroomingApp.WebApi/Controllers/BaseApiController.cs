namespace PetGroomingApp.WebApi.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected bool IsUserAuthenticated()
        {
            if (this.User.Identity != null)
            {
                return this.User.Identity.IsAuthenticated;
            }

            return false;
        }

        protected string? GetUserId()
        {
            if (this.IsUserAuthenticated())
            {
                return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return null;
        }
    }
}
