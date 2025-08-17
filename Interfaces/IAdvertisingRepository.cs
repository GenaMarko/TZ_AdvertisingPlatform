namespace TZ_AdvertisingPlatform.Interfaces
{
    public interface IAdvertisingRepository
    {
        void Clear();
        void Add(string location, string adName);
        bool TryGetAds(string location, out List<string> ads);
        int Count { get; }
    }
}
