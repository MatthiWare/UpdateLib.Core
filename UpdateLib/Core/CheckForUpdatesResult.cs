namespace UpdateLib.Core
{
    public class CheckForUpdatesResult
    {
        public bool UpdateAvailable { get; private set; }

        public UpdateVersion CurrentVersion { get; private set; }
        public UpdateVersion NewVersion { get; private set; }
    }
}
