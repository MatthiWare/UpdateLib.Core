using System;

namespace UpdateLib.Core.Common.IO
{
    public class FileEntry
    {
        public string Hash { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public FileEntry() { }

        public FileEntry(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
