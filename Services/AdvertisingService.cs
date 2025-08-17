using TZ_AdvertisingPlatform.Interfaces;
using TZ_AdvertisingPlatform.Models;

namespace TZ_AdvertisingPlatform.Services
{
    public class AdvertisingService : IAdvertisingService
    {
        private readonly IAdvertisingRepository _repository;
        private readonly IAdRecordValidator _validator;

        public AdvertisingService(IAdvertisingRepository repository, IAdRecordValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public LoadResult LoadFromFile(string filePath)
        {
            var result = new LoadResult();
            if (!File.Exists(filePath)) return result;

            _repository.Clear();

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!line.Contains(':'))
                {
                    result.UnreadableLines++;
                    continue;
                }

                var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    result.UnreadableLines++;
                    continue;
                }

                var adName = parts[0].Trim();
                var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim()).ToList();

                var record = new AdRecord(adName, locations);

                if (!_validator.Validate(record))
                {
                    result.UnreadableLines++;
                    continue;
                }

                foreach (var loc in record.Locations)
                {
                    _repository.Add(loc, record.Name);
                }
            }

            result.AddedCount = _repository.Count;
            return result;
        }

        public IReadOnlyCollection<string> GetByLocation(string location)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var current = location;

            while (!string.IsNullOrEmpty(current))
            {
                if (_repository.TryGetAds(current, out var ads))
                {
                    foreach (var ad in ads) result.Add(ad);
                }

                var lastSlash = current.LastIndexOf('/');
                if (lastSlash <= 0) break;
                current = current.Substring(0, lastSlash);
            }

            return result;
        }
    }
}
