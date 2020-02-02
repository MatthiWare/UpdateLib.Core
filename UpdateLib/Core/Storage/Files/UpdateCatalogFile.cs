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
    }
}
