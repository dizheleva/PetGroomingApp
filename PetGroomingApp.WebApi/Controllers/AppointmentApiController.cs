namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Data.Seeding.Dtos;
    using PetGroomingApp.Services.Core.Interfaces;
    using PetGroomingApp.Web.ViewModels.Appointment;

    public class AppointmentApiController : BaseApiController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IGroomerService _groomerService;

        public AppointmentApiController(IAppointmentService appointmentService,
            IServiceService serviceService, 
            IGroomerService groomerService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _groomerService = groomerService;
        }

        [HttpPost("Book")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [Authorize]
        public async Task<IActionResult> Book([FromBody] AppointmentUserFormViewModel model)
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
            return string.IsNullOrEmpty(result) ? BadRequest("Booking failed.") : Ok(result);
        }

        [HttpPost("CalculateTotals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CalculateTotals([FromBody] ServiceSelectionDto dto)
        {
            var result = await _serviceService.CalculateTotalsAsync(dto.SelectedServiceIds);
            return Ok(result);
        }


        [HttpPost("GetAvailableGroomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> GetAvailableGroomers([FromBody] AppointmentTimeDto dto)
        {
            var result = await _groomerService.GetAvailableGroomersAsync(dto.Time, dto.DurationMinutes);
            return Ok(result);
        }
    }
}
