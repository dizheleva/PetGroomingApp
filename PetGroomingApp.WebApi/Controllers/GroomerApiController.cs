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

        // GET: api/groomers/available?dateTime=2025-08-03T10:00
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableGroomers([FromQuery] DateTime dateTime, int serviceDurationMinutes)
        {
            var groomers = await _groomerService.GetAvailableGroomersAsync(dateTime, serviceDurationMinutes);
            return Ok(groomers);
        }

        // GET: api/groomers/{id}/available-times
        [HttpGet("{id}/available-times")]
        public async Task<IActionResult> GetGroomerAvailableTimes(string id, DateTime selectedTime)
        {
            var times = await _groomerService.GetAvailableTimesAsync(id, selectedTime);
            return Ok(times);
        }
    }
}
