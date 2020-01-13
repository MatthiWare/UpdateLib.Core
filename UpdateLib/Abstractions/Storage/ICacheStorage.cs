using System;
using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    public interface ICacheStorage
    {
        bool CacheExists { get; }
        Task SaveAsync(HashCacheFile file);
        Task<HashCacheFile> LoadAsync();
    }
}
