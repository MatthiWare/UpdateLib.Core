using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using UpdateLib.Abstractions.Common.IO;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core;
using UpdateLib.Core.Storage.Files;
using Xunit;
using static UpdateLib.Tests.Helpers;

namespace UpdateLib.Tests.Core
{
    public class UpdataCatalogManagerTests
    {
        [Fact]
        public async Task DownloadsRemoteFileWhenLocalFileDoesNotExist()
        {
            var downloadClient = new Mock<IDownloadClientService>();
            var storage = new Mock<IUpdateCatalogStorage>();
            var catalogus = new UpdateCatalogFile();

            storage.SetupGet(_ => _.Exists).Returns(true);
            downloadClient.Setup(_ => _.GetUpdateCatalogFileAsync()).ReturnsAsync(catalogus).Verifiable();

            var mgr = new UpdateCatalogManager(CreateLogger<UpdateCatalogManager>(), downloadClient.Object, storage.Object);

            var result = await mgr.GetUpdateCatalogFileAsync();

            Assert.Equal(catalogus, result);

            downloadClient.Verify();
        }

        [Fact]
        public async Task DownloadsRemoteFileWhenLocalFileDoesExistButIsTooOld()
        {
            var downloadClient = new Mock<IDownloadClientService>();
            var storage = new Mock<IUpdateCatalogStorage>();
            var catalogus = new UpdateCatalogFile();

            storage.SetupGet(_ => _.Exists).Returns(true);
            storage.SetupGet(_ => _.LastWriteTime).Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10)));
            downloadClient.Setup(_ => _.GetUpdateCatalogFileAsync()).ReturnsAsync(catalogus).Verifiable();

            var mgr = new UpdateCatalogManager(CreateLogger<UpdateCatalogManager>(), downloadClient.Object, storage.Object);

            var result = await mgr.GetUpdateCatalogFileAsync();

            Assert.Equal(catalogus, result);

            downloadClient.Verify();
        }

        [Fact]
        public async Task LoadsLocalFileWhenItExists()
        {
            var downloadClient = new Mock<IDownloadClientService>();
            var storage = new Mock<IUpdateCatalogStorage>();
            var catalogus = new UpdateCatalogFile();

            storage.SetupGet(_ => _.Exists).Returns(true);
            storage.SetupGet(_ => _.LastWriteTime).Returns(DateTime.UtcNow);
            storage.Setup(_ => _.LoadAsync()).ReturnsAsync(catalogus).Verifiable();

            var mgr = new UpdateCatalogManager(CreateLogger<UpdateCatalogManager>(), downloadClient.Object, storage.Object);

            var result = await mgr.GetUpdateCatalogFileAsync();

            Assert.Equal(catalogus, result);

            storage.Verify();
        }

        [Fact]
        public async Task DownloadsRemoteFileWhenLocalFileFailed()
        {
            var downloadClient = new Mock<IDownloadClientService>();
            var storage = new Mock<IUpdateCatalogStorage>();
            var catalogus = new UpdateCatalogFile();

            storage.SetupGet(_ => _.Exists).Returns(true);
            storage.SetupGet(_ => _.LastWriteTime).Returns(DateTime.UtcNow);
            storage.Setup(_ => _.LoadAsync()).Throws(new Exception()).Verifiable();
            downloadClient.Setup(_ => _.GetUpdateCatalogFileAsync()).ReturnsAsync(catalogus).Verifiable();

            var mgr = new UpdateCatalogManager(CreateLogger<UpdateCatalogManager>(), downloadClient.Object, storage.Object);

            var result = await mgr.GetUpdateCatalogFileAsync();

            Assert.Equal(catalogus, result);

            storage.Verify();
            downloadClient.Verify();
        }
    }
}
