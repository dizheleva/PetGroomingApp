namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;

    public class GroomerApiController : BaseApiController
    {
        private readonly IGroomerService _groomerService;

        public GroomerApiController(IGroomerService groomerService)
        {
            _groomerService = groomerService;
        }

        public class AvailableGroomersRequest
        {
            public DateTime AppointmentTime { get; set; }
            public int DurationMinutes { get; set; }
        }

        public class GroomerAvailableTimesRequest
        {
            public required string Id { get; set; }
            public int Duration { get; set; }
        }

        [HttpPost("AvailableGroomers")]
        public async Task<IActionResult> GetAvailableGroomers([FromBody] AvailableGroomersRequest request)
        {
            var groomers = await _groomerService.GetAvailableGroomersAsync(request.AppointmentTime, request.DurationMinutes);
            return Ok(groomers);
        }

        [HttpPost("AvailableTimes")]
        public async Task<IActionResult> GetGroomerAvailableTimes([FromBody] GroomerAvailableTimesRequest request)
        {
            var times = await _groomerService.GetAvailableTimesAsync(request.Id, request.Duration);
            return Ok(times);
        }
    }
}
