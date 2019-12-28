using System;
using System.Collections.Generic;

namespace UpdateLib.Core.Storage.Files
{
    public class HashCacheFile
    {
        public List<HashCacheEntry> Entries { get; set; }

        public HashCacheFile()
        {
            Entries = new List<HashCacheEntry>();
        }
    }
}
