namespace PetGroomingApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PetGroomingApp.Services.Core.Interfaces;

    public class PetApiController : BaseApiController
    {
        private readonly IPetService _petService;

        public PetApiController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet("GetByUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetPetsByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User ID is required.");
            }

            var pets = await _petService.GetPetsByUserAsync(userId);
            return Ok(pets);
        }
    }
}

