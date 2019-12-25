using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;
using static System.Environment;

namespace UpdateLib.Core.Storage
{
    public class CacheStorage : ICacheStorage
    {
        private const string CachePathName = "Cache";
        private const string CacheFileName = "FileCache.json";

        private readonly IFileSystem fs;
        private readonly string cachePath;

        public CacheStorage(IFileSystem storage)
        {
            this.fs = storage ?? throw new ArgumentNullException(nameof(storage));

            cachePath = GetFilePathAndEnsureCreated();
        }

        private string GetFilePathAndEnsureCreated()
        {
            // Use DoNotVerify in case LocalApplicationData doesn’t exist.
            string path = fs.Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData, SpecialFolderOption.DoNotVerify), "UpdateLib", CachePathName, CacheFileName);
            // Ensure the directory and all its parents exist.
            fs.Directory.CreateDirectory(path);

            return path;
        }

        public async Task<HashCacheFile> LoadAsync()
        {
            using (var reader = fs.File.OpenText(cachePath))
            {
                var contents = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<HashCacheFile>(contents);
            }
        }

        public async Task SaveAsync(HashCacheFile file)
        {
            using (var stream = fs.File.OpenWrite(cachePath))
            using (var writer = new StreamWriter(stream))
            {
                var contents = JsonConvert.SerializeObject(file);
                await writer.WriteAsync(contents);
            }
        }
    }
}
