
using System.Threading.Tasks;
using UpdateLib.Core;

namespace UpdateLib.Abstractions
{
    interface IUpdater
    {
        bool IsInitialized { get; }
        Task<CheckForUpdatesResult> CheckForUpdatesAsync();
        Task InitializeAsync();
    }
}
