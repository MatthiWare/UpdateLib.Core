namespace UpdateLib.Core.Storage.Files
{
    public class HashCacheEntry
    {
        public HashCacheEntry(string name, long ticks, string hash)
        {
            FilePath = name;
            Ticks = ticks;
            Hash = hash;
        }

        public long Ticks { get; set; }
        public string FilePath { get; set; }
        public string Hash { get; set; }
    }
}
