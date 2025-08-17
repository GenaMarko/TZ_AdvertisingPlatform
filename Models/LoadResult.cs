namespace TZ_AdvertisingPlatform.Models
{
    public class LoadResult
    {
        public int AddedCount { get; set; }
        public int UnreadableLines { get; set; }
        public bool IsEmpty => AddedCount == 0;
    }
}
