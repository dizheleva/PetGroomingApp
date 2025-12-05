namespace PetGroomingApp.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class GroomerApiController : ControllerBase
    {
        private readonly IGroomerService _groomerService;

        public GroomerApiController(IGroomerService groomerService)
        {
            _groomerService = groomerService;
        }

        [HttpPost("AvailableTimes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroomerAvailableTimes([FromBody] GroomerAvailableTimesRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Id) || request.Duration <= 0)
            {
                return BadRequest("Groomer ID and duration are required.");
            }

            var times = await _groomerService.GetAvailableTimesAsync(request.Id, request.Duration);
            return Ok(times);
        }

        public class GroomerAvailableTimesRequest
        {
            public required string Id { get; set; }
            public int Duration { get; set; }
        }
    }
}

