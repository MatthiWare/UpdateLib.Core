using System.IO.Abstractions;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Core.Storage
{
    public class CacheStorage : BaseStorage<HashCacheFile>, ICacheStorage
    {
        private const string CachePathName = "Cache";
        private const string CacheFileName = "FileCache.json";

        public bool CacheExists => FileInfo.Exists;

        public CacheStorage(IFileSystem storage)
            : base(storage, "UpdateLib", CachePathName, CacheFileName)
        {
        }
    }
}
