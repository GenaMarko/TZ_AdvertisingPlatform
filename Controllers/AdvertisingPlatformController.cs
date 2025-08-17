using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TZ_AdvertisingPlatform.Interfaces;

namespace TZ_AdvertisingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisingPlatformController : ControllerBase
    {
        private readonly IAdvertisingService _service;

        public AdvertisingPlatformController(IAdvertisingService service)
        {
            _service = service;
        }

        [HttpPost("Load")]
        public IActionResult LoadFromFile()
        {
            var result = _service.LoadFromFile("data.txt");

            if (result.IsEmpty)
                return NotFound("Файл пустой или нет читаемых строк.");

            if (result.UnreadableLines > 0)
                return Ok($"Загружено {result.AddedCount} строк, нечитаемых: {result.UnreadableLines}");

            return Ok($"Загружено {result.AddedCount} строк");
        }

        [HttpGet("GetByLocation")]
        public IActionResult GetByLocation(string location)
        {
            var result = _service.GetByLocation(location);
            if (result.Count == 0)
                return NotFound($"По запросу '{location}' ничего не найдено");

            return Ok(result);
        }
    }
}
