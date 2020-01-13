using System;
using System.Collections.Generic;
using System.Linq;
using UpdateLib.Core.Common;

namespace UpdateLib.Core.Storage.Files
{
    public class UpdateCatalogFile
    {
        /// <summary>
        /// Gets the <see cref="UpdateInfo"/> Catalog
        /// </summary>
        public List<UpdateInfo> Catalog { get; private set; } = new List<UpdateInfo>();

        /// <summary>
        /// Download Url's 
        /// </summary>
        public List<string> DownloadUrls { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the best update for the current version. 
        /// </summary>
        /// <param name="currentVersion">The currect application version</param>
        /// <returns><see cref="UpdateInfo"/></returns>
        public UpdateInfo GetLatestUpdateForVersion(UpdateVersion currentVersion)
        {
            if (currentVersion == null) throw new ArgumentNullException(nameof(currentVersion));

            return Catalog.OrderBy(c => c).Where(c => currentVersion < c.Version && ((c.IsPatch && c.BasedOnVersion == currentVersion) || !c.IsPatch)).FirstOrDefault();
        }
    }
}
