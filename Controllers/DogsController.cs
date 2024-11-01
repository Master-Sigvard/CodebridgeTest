using CodebridgeTest.Models;
using Microsoft.AspNetCore.Mvc;
using CodebridgeTest.Services;
using CodebridgeTest.Services.Interfaces;

namespace CodebridgeTest.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogsService _dogService;

        public DogsController(IDogsService dogService)
        {
            _dogService = dogService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }

        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog([FromBody] Dogs dog)
        {
            try
            {
                await _dogService.CreateDogAsync(dog);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("dogs")]
        public async Task<IActionResult> GetDogs(
        string attribute = "name",
        string order = "asc",
        int pageNumber = 1,
        int pageSize = 10)
        {
            var dogs = await _dogService.GetDogsAsync(attribute, order, pageNumber, pageSize);
            return Ok(dogs);
        }
    }
}
