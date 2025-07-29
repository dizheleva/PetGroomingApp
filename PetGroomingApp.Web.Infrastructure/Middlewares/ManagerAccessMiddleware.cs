namespace PetGroomingApp.Web.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Http;

    public class ManagerAccessMiddleware
    {
        private readonly RequestDelegate _next;
        public ManagerAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            if (path.StartsWith("/manager"))
            {
                if (context.User.Identity?.IsAuthenticated == false || !context.User.IsInRole("Manager"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.Redirect("/Home/AccessDenied");
                    return;
                }
            }

            await _next(context);
        }
    }
}
