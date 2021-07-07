using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using UpdateLib.Core.Common.IO;
using UpdateLib.Core.Storage;
using UpdateLib.Core.Storage.Files;
using Xunit;

namespace UpdateLib.Tests.Core.Storage
{
    public class UpdateFileStorageTests
    {
        [Fact]
        public async Task SaveAndLoadAreTheSame()
        {
            var mockFileSystem = new MockFileSystem();

            var storage = new UpdateFileStorage(mockFileSystem);
            var file = new UpdateFile();

            var dir = new DirectoryEntry("dir");
            var fileEntry = new FileEntry("file.txt");

            dir.Add(fileEntry);

            file.Entries.Add(dir);

            await storage.SaveAsync(file);

            var loadedFile = await storage.LoadAsync();

            var loadedEntry = loadedFile.Entries.First().Files.First();

            Assert.Equal(fileEntry.Hash, loadedEntry.Hash);
            Assert.Equal(fileEntry.Name, loadedEntry.Name);
            Assert.Equal(fileEntry.Path, loadedEntry.Path);
        }
    }
}
