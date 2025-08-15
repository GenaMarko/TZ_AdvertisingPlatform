using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace TZ_AdvertisingPlatform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdvertisingPlatformController : ControllerBase
    {
        private readonly AdvertisingPlatformStorage _storage;

        public AdvertisingPlatformController(AdvertisingPlatformStorage storage) 
        { 
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

            _storage.Data.Clear();

            foreach (var line in System.IO.File.ReadAllLines(filePath))
            {
                var parts = line.Split(':',StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    continue;
                }

                var adName = parts[0].Trim();

                var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).
                    Select(x => x.Trim());

                foreach (var loc in locations)
                {
                    if (!_storage.Data.ContainsKey(loc))
                    {
                        _storage.Data[loc] = new List<string>();
                    }

                    _storage.Data[loc].Add(adName);
                }
            }

            return Ok($"Данные успешно загружены в количестве {_storage.Data.Count}");
        }

        [HttpGet("Get advertisement by location")]
        public IEnumerable<string> GetByLocation(string location)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var current = location;

            while (!string.IsNullOrEmpty(current))
            {
                if (_storage.Data.TryGetValue(current,out var ads))
                {
                    foreach (var ad in ads)
                    {
                        result.Add(ad);
                    }
                }

                var lastSlash = current.LastIndexOf('/');

                if (lastSlash <= 0)
                {
                    break;
                }

                current = current.Substring(0, lastSlash);
            }

            return result;

        }
    }
}
