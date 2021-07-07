using System;
using System.Collections.Generic;

namespace UpdateLib.Core.Storage.Files
{
    public class HashCacheFile
    {
        public UpdateVersion Version { get; set; }
        public List<HashCacheEntry> Entries { get; set; }

        public HashCacheFile()
        {
            Entries = new List<HashCacheEntry>();
        }
    }
}
