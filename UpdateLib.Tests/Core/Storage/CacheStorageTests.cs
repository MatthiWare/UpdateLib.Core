using System;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using UpdateLib.Core.Storage;
using UpdateLib.Core.Storage.Files;
using Xunit;

namespace UpdateLib.Tests.Core.Storage
{
    public class CacheStorageTests
    {
        [Fact]
        public async Task CacheSaveAndLoadAreTheSame()
        {
            var mockFileSystem = new MockFileSystem();

            var cache = new CacheStorage(mockFileSystem);
            var file = new HashCacheFile();
            var entry = new HashCacheEntry("name", DateTime.UtcNow.Ticks, "some hash");

            file.Entries.Add(entry);

            await cache.SaveAsync(file);

            var loadedFile = await cache.LoadAsync();

            var loadedEntry = loadedFile.Entries.First();

            Assert.Equal(entry.FilePath, loadedEntry.FilePath);
            Assert.Equal(entry.Hash, loadedEntry.Hash);
            Assert.Equal(entry.Ticks, loadedEntry.Ticks);
        }
    }
}
