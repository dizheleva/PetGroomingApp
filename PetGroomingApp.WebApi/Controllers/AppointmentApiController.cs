namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;
        
    public class AppointmentApiController : BaseApiController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentApiController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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

            var result = await _appointmentService.CreateAsync(model, userId);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to book appointment.");
            }

            return Ok(result);
        }

        //[HttpPost("CheckAvailableGroomers")]
        //public async Task<IActionResult> CheckAvailableGroomers([FromBody] DateTime appointmentTime)
        //{
        //    var groomers = await _appointmentService.GetAvailableGroomersAsync(appointmentTime);
        //    return Ok(groomers);
        //}

        //[HttpGet("GetGroomerNextAvailability/{groomerId}")]
        //public async Task<IActionResult> GetGroomerNextAvailability(Guid groomerId)
        //{
        //    var time = await _appointmentService.GetNextFreeTimeForGroomerAsync(groomerId);
        //    return Ok(time);
        //}

        //[HttpPost("CalculateTotal")] // from list of selected serviceIds
        //public async Task<IActionResult> CalculateTotal([FromBody] List<Guid> serviceIds)
        //{
        //    var result = await _appointmentService.CalculateTotalAsync(serviceIds);
        //    return Ok(result);
        //}
    }
}
