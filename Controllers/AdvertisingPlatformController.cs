using Microsoft.AspNetCore.Mvc;

namespace TZ_AdvertisingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisingPlatformController : ControllerBase
    {
        private readonly ILogger<AdvertisingPlatformController> _logger;
        private readonly AdvertisingPlatformStorage _storage;

        public AdvertisingPlatformController(
            ILogger<AdvertisingPlatformController> logger, 
            AdvertisingPlatformStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpPut("Load")]
        public IActionResult LoadFromFile()
        {
            string filePath = "data.txt";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Файл {filePath} не найден");
            }

            foreach (var line in System.IO.File.ReadAllLines(filePath))
            {
                var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    _storage.Data[parts[0]] = parts[1];
                }
            }

            return Ok($"Данные успешно загружены в количестве {_storage.Data.Count}");
        }

        [HttpGet("GetAll")]
        public IEnumerable<AdvertisingPlatforms> GetAll()
        {
            return _storage.Data.Select(x => new AdvertisingPlatforms
            {
                Advertisement = x.Key,
                Location = x.Value
            });
        }
    }
}
