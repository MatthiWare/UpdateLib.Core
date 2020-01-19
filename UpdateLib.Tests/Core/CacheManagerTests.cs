using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core;
using UpdateLib.Core.Storage;
using UpdateLib.Core.Storage.Files;
using Xunit;
using static System.Environment;
using static UpdateLib.Tests.Helpers;

namespace UpdateLib.Tests.Core
{
    public class CacheManagerTests
    {
        [Fact]
        public async Task NonExistingCacheCreatesANewOne()
        {
            var fs = new MockFileSystem();
            var cache = new CacheStorage(fs);
            var manager = CreateCacheManager(fs, cache);

            await manager.UpdateCacheAsync();

            Assert.NotNull(await cache.LoadAsync());
        }

        [Fact]
        public async Task OldCacheEntriesAreDeleted()
        {
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>(), "C:\\app");
            var cache = new CacheStorage(fs);

            var file = new HashCacheFile();
            file.Entries.Add(new HashCacheEntry("name", 0, ""));

            await cache.SaveAsync(file);

            var manager = CreateCacheManager(fs, cache);

            await manager.UpdateCacheAsync();

            var result = await cache.LoadAsync();

            Assert.Empty(result.Entries);
        }

        [Fact]
        public async Task UpdateCacheAddsNewEntries()
        {
            var fs = new MockFileSystem();
            fs.AddFile("./myfile.txt", new MockFileData("blabla"));

            var cache = new CacheStorage(fs);
            var manager = CreateCacheManager(fs, cache);

            await manager.UpdateCacheAsync();

            var result = (await cache.LoadAsync()).Entries.First();

            Assert.Equal(fs.Path.GetFullPath("./myfile.txt"), result.FilePath);
        }

        [Fact]
        public async Task UpdateCacheAddsNewEntries_TempFilesAreIgnored()
        {
            var fs = new MockFileSystem();
            fs.AddFile("./myfile.txt", new MockFileData("blabla"));
            fs.AddFile("./myfile.txt.old.tmp", new MockFileData("blabla"));
            fs.AddFile("./someOtherFile.txt.old.tmp", new MockFileData("blabla"));

            var cache = new CacheStorage(fs);
            var manager = CreateCacheManager(fs, cache);

            await manager.UpdateCacheAsync();

            Assert.Single((await cache.LoadAsync()).Entries);
        }

        [Fact]
        public async Task CorruptCacheFileGetsRestored()
        {
            var fs = new MockFileSystem();
            var cache = new CacheStorage(fs);

            fs.AddFile(cache.FileInfo.FullName, new MockFileData("blabla")); // not valid json

            var manager = CreateCacheManager(fs, cache);

            await manager.UpdateCacheAsync();
        }

        private CacheManager CreateCacheManager(IFileSystem fs, ICacheStorage storage)
            => new CacheManager(fs, storage, CreateLogger<CacheManager>());
    }
}
