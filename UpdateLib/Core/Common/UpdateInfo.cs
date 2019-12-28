using System;
using Newtonsoft.Json;

namespace UpdateLib.Core.Common
{
    public class UpdateInfo : IComparable, IComparable<UpdateInfo>
    {
        public UpdateVersion BasedOnVersion { get; set; }
        public UpdateVersion Version { get; set; }
        public string FileName { get; set; }
        public string Hash { get; set; }

        [JsonIgnore]
        public bool IsPatch => BasedOnVersion != null;

        public UpdateInfo() { }

        /// <summary>
        /// A new catalog entry
        /// </summary>
        /// <param name="version">The update version</param>
        /// <param name="basedOnVersion">The version this update is based on, can be null if it's not a patch.</param>
        /// <param name="fileName">The file name for the update.</param>
        /// <param name="hash">The calculated hash for the update</param>
        public UpdateInfo(UpdateVersion version, UpdateVersion basedOnVersion, string fileName, string hash)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Hash = hash ?? throw new ArgumentNullException(nameof(hash));

            BasedOnVersion = basedOnVersion;

            if (version <= basedOnVersion) throw new ArgumentOutOfRangeException(nameof(basedOnVersion), "The new version cannot be smaller than the version it was based on.");
        }

        public int CompareTo(UpdateInfo other)
        {
            if (other == null) return -1;

            if (Version > other.Version) return -1;

            if (Version == other.Version)
            {
                if (IsPatch && other.IsPatch) return BasedOnVersion.CompareTo(other.BasedOnVersion);

                if (IsPatch && !other.IsPatch) return -1;

                if (!IsPatch && other.IsPatch) return 1;

                return 0;
            }

            return 1;
        }

        public int CompareTo(object obj) => CompareTo(obj as UpdateInfo);
    }
}
