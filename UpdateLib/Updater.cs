using System;
using System.Threading.Tasks;
using UpdateLib.Abstractions;
using UpdateLib.Core;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib
{
    public class Updater : IUpdater
    {
        private readonly ICacheManager cacheManager;
        private readonly IUpdateCatalogManager updateCatalogManager;
        private HashCacheFile cacheFile;

        public bool IsInitialized { get; private set; }

        public Updater(ICacheManager cacheManager, IUpdateCatalogManager updateCatalogManager)
        {
            this.cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            this.updateCatalogManager = updateCatalogManager ?? throw new ArgumentNullException(nameof(updateCatalogManager));
        }

        public async Task<CheckForUpdatesResult> CheckForUpdatesAsync()
        {
            if (!IsInitialized)
                await InitializeAsync();

            var catalogFile = await updateCatalogManager.GetUpdateCatalogFileAsync();

            var updateInfo = updateCatalogManager.GetLatestUpdateForVersion(cacheFile.Version, catalogFile);

            return new CheckForUpdatesResult(updateInfo);
        }

        public async Task InitializeAsync()
        {
            cacheFile = await cacheManager.UpdateCacheAsync();

            IsInitialized = true;
        }
    }
}
