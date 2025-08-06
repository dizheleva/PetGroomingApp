namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
        
    public class ServiceApiController : BaseApiController
    {
        private readonly IServiceService _serviceService;

        public ServiceApiController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost("totalDuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> GetTotalServicesDuration([FromBody] List<string> serviceIds)
        {
            var result = await _serviceService.GetTotalDurationAsync(serviceIds);
            return Ok(result); 
        }

        [HttpPost("totalPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> GetTotalServicesPrice([FromBody] List<string> serviceIds)
        {
            var result = await _serviceService.GetTotalPriceAsync(serviceIds);
            return Ok(result); 
        }
    }
}
