namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentApiController : BaseApiController
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentApiController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("Book")]
        [Authorize]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentFormViewModel model)
        {
            if (!this.IsUserAuthenticated())
            {
                return Unauthorized();
            }
            if (model == null)
            {
                return BadRequest("Appointment model is required.");
            }

            var userId = this.GetUserId();
            if (userId == null)
            {
                return BadRequest("User ID is required.");
            }

            var result = await this.appointmentService.CreateAsync(model, userId);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to book appointment.");
            }

            return Ok(result);
        }
    }
}
