using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UpdateLib.Abstractions;
using UpdateLib.Abstractions.Common.IO;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Core
{
    public class UpdateCatalogManager : IUpdateCatalogManager
    {
        private readonly ILogger<UpdateCatalogManager> logger;
        private readonly IDownloadClientService downloadClient;
        private readonly IUpdateCatalogStorage storage;

        public UpdateCatalogManager(ILogger<UpdateCatalogManager> logger, IDownloadClientService downloadClient, IUpdateCatalogStorage storage)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.downloadClient = downloadClient ?? throw new ArgumentNullException(nameof(downloadClient));
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<UpdateCatalogFile> GetUpdateCatalogFileAsync()
        {
            logger.LogInformation("Getting update catalog file");

            if (!storage.Exists || storage.LastWriteTime.AddMinutes(5) < DateTime.UtcNow)
            {
                logger.LogDebug("Downloading remote update catalog file");

                return await DownloadRemoteCatalogFileAsync();
            }
            else
            {
                logger.LogDebug("Loading local update catalog file");

                return await storage.LoadAsync();
            }
        }

        private async Task<UpdateCatalogFile> DownloadRemoteCatalogFileAsync()
        {
            var file = await downloadClient.GetUpdateCatalogFileAsync();

            await storage.SaveAsync(file);

            return file;
        }
    }
}
