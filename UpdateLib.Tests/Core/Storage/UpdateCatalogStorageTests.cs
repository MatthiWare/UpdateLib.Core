using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using UpdateLib.Core.Common;
using UpdateLib.Core.Storage;
using UpdateLib.Core.Storage.Files;
using Xunit;

namespace UpdateLib.Tests.Core.Storage
{
    public class UpdateCatalogStorageTests
    {
        [Fact]
        public async Task SaveAndLoadAreTheSame()
        {
            var mockFileSystem = new MockFileSystem();

            var storage = new UpdateCatalogStorage(mockFileSystem);
            var file = new UpdateCatalogFile();
            var info = new UpdateInfo("1.0.0", null, "name", "hash");

            file.Catalog.Add(info);

            await storage.SaveAsync(file);

            var loadedFile = await storage.LoadAsync();

            var loadedEntry = loadedFile.Catalog.First();

            Assert.Equal(info.FileName, loadedEntry.FileName);
            Assert.Equal(info.Hash, loadedEntry.Hash);
            Assert.Equal(info.IsPatch, loadedEntry.IsPatch);
            Assert.Equal(info.Version, loadedEntry.Version);
            Assert.Equal(info.BasedOnVersion, loadedEntry.BasedOnVersion);
        }
    }
}
