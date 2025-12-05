namespace PetGroomingApp.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Data.Seeding.Dtos;
    using PetGroomingApp.Services.Core.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentApiController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public AppointmentApiController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost("CalculateTotals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> CalculateTotals([FromBody] ServiceSelectionDto dto)
        {
            if (dto == null || dto.SelectedServiceIds == null || dto.SelectedServiceIds.Count == 0)
            {
                return BadRequest("Service IDs are required.");
            }

            var result = await _serviceService.CalculateTotalsAsync(dto.SelectedServiceIds);
            return Ok(result);
        }
    }
}

