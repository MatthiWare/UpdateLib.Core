using UpdateLib.Core.Common;

namespace UpdateLib.Core
{
    public class CheckForUpdatesResult
    {
        public bool UpdateAvailable { get; private set; }
        public UpdateVersion NewVersion { get; private set; }

        public CheckForUpdatesResult(UpdateInfo info)
        {
            UpdateAvailable = info != null;
            NewVersion = info?.Version;
        }
    }
}
