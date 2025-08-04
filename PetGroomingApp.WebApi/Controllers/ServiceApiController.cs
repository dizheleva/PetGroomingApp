namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;
        
    public class ServiceApiController : BaseApiController
    {
        private readonly IServiceService _serviceService;

        public ServiceApiController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // POST: api/services/totalDuration
        [HttpPost("totalDuration")]
        public async Task<IActionResult> GetTotalServicesDuration([FromBody] List<string> serviceIds)
        {
            var result = await _serviceService.GetTotalDurationAsync(serviceIds);
            return Ok(result); 
        }

        // POST: api/services/totalPrice
        [HttpPost("totalPrice")]
        public async Task<IActionResult> GetTotalServicesPrice([FromBody] List<string> serviceIds)
        {
            var result = await _serviceService.GetTotalPriceAsync(serviceIds);
            return Ok(result); 
        }
    }
}
