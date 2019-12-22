using System.Threading.Tasks;
using UpdateLib.Abstractions;
using UpdateLib.Core;

namespace UpdateLib
{
    public class Updater : IUpdater
    {
        public CheckForUpdatesResult CheckForUpdates()
        {
            throw new System.NotImplementedException();
        }

        public Task<CheckForUpdatesResult> CheckForUpdatesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
