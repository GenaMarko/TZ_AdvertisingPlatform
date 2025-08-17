using TZ_AdvertisingPlatform.Models;

namespace TZ_AdvertisingPlatform.Interfaces
{
    public interface IAdRecordValidator
    {
        bool Validate(AdRecord record);
    }
}
