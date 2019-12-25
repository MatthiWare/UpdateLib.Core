using System.Threading.Tasks;
using UpdateLib.Abstractions;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core;

namespace UpdateLib
{
    public class Updater : IUpdater
    {
        private readonly ICacheStorage cacheStorage;

        public Updater(ICacheStorage cacheStorage)
        {
            this.cacheStorage = cacheStorage ?? throw new System.ArgumentNullException(nameof(cacheStorage));
        }

        public Task<CheckForUpdatesResult> CheckForUpdatesAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task InitializeAsync()
        {
            var cache = await cacheStorage.LoadAsync();
        }
    }
}
