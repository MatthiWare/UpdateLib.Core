
using System.Threading.Tasks;
using UpdateLib.Core;

namespace UpdateLib.Abstractions
{
    interface IUpdater
    {
        Task<CheckForUpdatesResult> CheckForUpdatesAsync();
        CheckForUpdatesResult CheckForUpdates();
    }
}
