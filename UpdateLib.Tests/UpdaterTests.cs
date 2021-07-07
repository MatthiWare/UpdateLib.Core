using Moq;
using System.Threading.Tasks;
using UpdateLib.Abstractions;
using Xunit;

namespace UpdateLib.Tests
{
    public class UpdaterTests
    {
        [Fact]
        public async Task InitializeShouldReturnTrueOnceInitialized()
        {
            var cacheManagerMock = new Mock<ICacheManager>(MockBehavior.Loose);
            var catalogManagerMock = new Mock<IUpdateCatalogManager>(MockBehavior.Loose);

            var updater = new Updater(cacheManagerMock.Object, catalogManagerMock.Object);

            await updater.InitializeAsync();

            Assert.True(updater.IsInitialized);
        }

        [Fact]
        public async Task InitializeShouldReturnTrueAfterCheckForUpdates()
        {
            var cacheManagerMock = new Mock<ICacheManager>(MockBehavior.Loose);
            var catalogManagerMock = new Mock<IUpdateCatalogManager>(MockBehavior.Loose);

            cacheManagerMock
                .Setup(_ => _.UpdateCacheAsync())
                .ReturnsAsync(new UpdateLib.Core.Storage.Files.HashCacheFile());

            catalogManagerMock
                .Setup(_ => _.GetUpdateCatalogFileAsync())
                .ReturnsAsync(new UpdateLib.Core.Storage.Files.UpdateCatalogFile());

            var updater = new Updater(cacheManagerMock.Object, catalogManagerMock.Object);

            await updater.CheckForUpdatesAsync();

            Assert.True(updater.IsInitialized);
        }
    }
}
