using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    public interface ICacheStorage : IStorage<HashCacheFile>
    {
        bool CacheExists { get; }
    }
}
