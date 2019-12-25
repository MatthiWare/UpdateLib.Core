using System;
using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    public interface ICacheStorage
    {
        Task SaveAsync(HashCacheFile file);
        Task<HashCacheFile> LoadAsync();
    }
}
