using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UpdateLib.Abstractions;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib
{
    public class Updater : IUpdater
    {
        private readonly ICacheManager cacheManager;
        private HashCacheFile cacheFile;

        public bool IsInitialized { get; private set; }

        public Updater(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        public async Task<CheckForUpdatesResult> CheckForUpdatesAsync()
        {
            if (!IsInitialized)
                await InitializeAsync();

            IHttpClientFactory factory;
        }

        public async Task InitializeAsync()
        {
            cacheFile = await cacheManager.UpdateCacheAsync();

            IsInitialized = true;
        }
    }
}
