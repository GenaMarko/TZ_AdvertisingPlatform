using TZ_AdvertisingPlatform.Interfaces;

namespace TZ_AdvertisingPlatform.Services
{
    public class InMemoryAdvertisingRepository : IAdvertisingRepository
    {
        private readonly Dictionary<string, List<string>> _data = new();

        public void Clear() => _data.Clear();

        public void Add(string location, string adName)
        {
            if (!_data.ContainsKey(location))
            {
                _data[location] = new List<string>();
            }
            _data[location].Add(adName);
        }

        public bool TryGetAds(string location, out List<string> ads)
            => _data.TryGetValue(location, out ads);

        public int Count => _data.Count;
    }
}
