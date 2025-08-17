using System.IO.Pipelines;
using TZ_AdvertisingPlatform.Models;

namespace TZ_AdvertisingPlatform.Interfaces
{
    public interface IAdvertisingService
    {
        LoadResult LoadFromFile(string filePath);
        IReadOnlyCollection<string> GetByLocation(string location);
    }
}
