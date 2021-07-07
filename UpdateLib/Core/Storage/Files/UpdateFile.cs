using System;
using System.Collections.Generic;
using System.Linq;
using UpdateLib.Core.Common.IO;

namespace UpdateLib.Core.Storage.Files
{
    public class UpdateFile
    {
        public List<DirectoryEntry> Entries { get; set; } = new List<DirectoryEntry>();

        public int FileCount => Entries.Sum(dir => dir.Count);
    }
}
