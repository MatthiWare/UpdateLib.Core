using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateLib.Core.Common.IO
{
    public class FileEntry
    {
        public string Hash { get; set; }

        public string Name { get; set; }
        
        public DirectoryEntry Parent { get; set; }

        public FileEntry() { }

        public FileEntry(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public FileEntry(string name, DirectoryEntry parent)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public string SourceLocation
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(Parent?.SourceLocation ?? string.Empty);
                sb.Append(Name);

                return sb.ToString();
            }
        }

        public string DestinationLocation
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(Parent?.DestinationLocation ?? string.Empty);
                sb.Append(Name);

                return sb.ToString();
            }
        }
    }
}
