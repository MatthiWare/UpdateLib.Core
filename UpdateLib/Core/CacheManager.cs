using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UpdateLib.Abstractions;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Core
{
    public class CacheManager : ICacheManager
    {
        private readonly IFileSystem fs;
        private readonly ICacheStorage cacheStorage;
        private readonly ILogger<CacheManager> logger;
        private IEnumerable<IFileInfo> files;

        public CacheManager(IFileSystem fs, ICacheStorage cacheStorage, ILogger<CacheManager> logger)
        {
            this.fs = fs ?? throw new ArgumentNullException(nameof(fs));
            this.cacheStorage = cacheStorage ?? throw new ArgumentNullException(nameof(cacheStorage));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task UpdateCacheAsync()
        {
            HashCacheFile file = null;

            try
            {
                file = await cacheStorage.LoadAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to load cache from storage");
            }

            files = fs.DirectoryInfo.FromDirectoryName(".").GetFiles("*", SearchOption.AllDirectories).Where(f => !f.FullName.Contains(".old.tmp"));

            logger.LogDebug($"Found {files.Count()} to recheck.");

            if (file == null)
            {
                file = await CreateNewHashCacheFileAsync();
                return;
            }

            await UpdateExistingFiles(file);

            await cacheStorage.SaveAsync(file);
        }

        private async Task UpdateExistingFiles(HashCacheFile cacheFile)
        {
            Dictionary<HashCacheEntry, bool> existingEntries = new Dictionary<HashCacheEntry, bool>(cacheFile.Entries.Count);

            foreach (var entry in cacheFile.Entries)
            {
                existingEntries.Add(entry, false);
            }

            foreach (var file in files)
            {
                var entry = cacheFile.Entries.FirstOrDefault(match => match.FilePath == file.FullName);

                HashCacheEntry newEntry = null;

                try
                {
                    newEntry = await CreateNewEntry(file).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Unable to create cache entry for {file.FullName}. The file might be in user or no longer exists.");
                }

                if (newEntry == null)
                    continue;

                if (entry != null)
                {
                    existingEntries[entry] = true;

                    entry.Hash = newEntry.Hash;
                    entry.Ticks = newEntry.Ticks;
                }
                else
                {
                    cacheFile.Entries.Add(newEntry);
                }
            }

            existingEntries.Where(item => !item.Value).Select(item => item.Key).ForEach(entry => cacheFile.Entries.Remove(entry));
        }

        private async Task<HashCacheFile> CreateNewHashCacheFileAsync()
        {
            var result = new HashCacheFile();

            foreach (var f in files)
            {
                try
                {
                    var entry = await CreateNewEntry(f).ConfigureAwait(false);

                    result.Entries.Add(entry);

                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Unable to create cache entry for {f.FullName}. The file might be in user or no longer exists.");
                }
            }

            await cacheStorage.SaveAsync(result);

            return result;
        }

        private async Task<HashCacheEntry> CreateNewEntry(IFileInfo fileInfo)
        {
            using (var stream = fileInfo.OpenRead())
            {
                var hash = await stream.GetHashAsync<SHA256CryptoServiceProvider>();
                var ticks = fileInfo.LastWriteTimeUtc.Ticks;
                var name = fileInfo.FullName;

                return new HashCacheEntry(name, ticks, hash);
            }
        }
    }
}
