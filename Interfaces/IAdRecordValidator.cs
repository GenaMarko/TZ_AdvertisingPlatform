using TZ_AdvertisingPlatform.Models;

namespace TZ_AdvertisingPlatform.Interfaces
{
    //Абстракция валидации данных(локации и связанных с ней рекламных площадок)
    public interface IAdRecordValidator
    {
        bool Validate(AdRecord record);
    }
}
