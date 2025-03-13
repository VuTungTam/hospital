namespace Hospital.SharedKernel.Caching.Models
{
    public class CacheEntry
    {
        public string Key { get; set; }

        public int ExpiriesInSeconds { get; set; }

        public CacheEntry(string key, int expiriesInSeconds)
        {
            Key = key;
            ExpiriesInSeconds = expiriesInSeconds;
        }
    }
}
