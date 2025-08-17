using TZ_AdvertisingPlatform.Interfaces;
using TZ_AdvertisingPlatform.Models;

namespace TZ_AdvertisingPlatform.Services
{
    //Класс для проверки элемента словаря
    public class AdRecordValidator : IAdRecordValidator
    {
        public bool Validate(AdRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.Name)) return false;
            if (record.Locations.Count == 0) return false;
            return record.Locations.All(loc =>
                !string.IsNullOrWhiteSpace(loc) &&
                loc.StartsWith('/') &&
                !loc.Contains("//") &&
                !loc.Contains(" "));
        }
    }
}
