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

            int unreadableLine = 0;

            foreach (var line in System.IO.File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!line.Contains(':'))
                {
                    unreadableLine++;
                    continue;
                }

                var parts = line.Split(':',StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    unreadableLine++;
                    continue;
                }

                var adName = parts[0].Trim();

                if (string.IsNullOrWhiteSpace(adName))
                {
                    unreadableLine++;
                    continue;
                }

                var locations = parts[1].
                    Split(',', StringSplitOptions.RemoveEmptyEntries).
                    Select(x => x.Trim()).
                    ToList();

                if (locations.Count == 0)
                {
                    unreadableLine++;
                    continue;
                }

                bool locIsValid = true;

                foreach (var loc in locations)
                {
                    if (string.IsNullOrWhiteSpace(loc) || 
                        !loc.StartsWith('/') || 
                        loc.Contains("//") ||
                        loc.Contains(" "))
                    {
                        locIsValid = false;
                        break;
                    }
                }

                if (!locIsValid)
                {
                    unreadableLine++;
                    continue;
                }

                foreach (var loc in locations)
                {
                    if (!_storage.Data.ContainsKey(loc))
                    {
                        _storage.Data[loc] = new List<string>();
                    }

                    _storage.Data[loc].Add(adName);
                }
            }

            if (_storage.Data.Count == 0)
            {
                return NotFound("Отсутствуют читаемые строки.");
            }

            if (unreadableLine > 0)
            {
                return Ok($"Не все данные прочитаны.\n" +
                    $"Количество добавленных данных: {_storage.Data.Count}\n" +
                    $"Количество нечитаемых строк: {unreadableLine}");
            }

            return Ok($"Данные успешно загружены в количестве {_storage.Data.Count}");
        }

        [HttpGet("GetByLocation")]
        public IActionResult GetByLocation(string location)
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

            if (result.Count == 0)
            {
                return NotFound($"По запросу '{current}' ничего не найдено");
            }

            return Ok(result);
        }
    }
}
