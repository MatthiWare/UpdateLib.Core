using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateLib.Core.Common.IO
{
    public class DirectoryEntry
    {
        private readonly List<DirectoryEntry> directories = new List<DirectoryEntry>();
        private readonly List<FileEntry> files = new List<FileEntry>();

        public string Name { get; set; }

        public int Count => Files.Count + Directories.Sum(d => d.Count);
        public IReadOnlyList<DirectoryEntry> Directories => directories.AsReadOnly();
        public IReadOnlyList<FileEntry> Files => files.AsReadOnly();
        public DirectoryEntry Parent { get; set; }

        public string SourceLocation
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (Parent == null)
                    return string.Empty;

                sb.Append(Parent.SourceLocation);
                sb.Append(Name);
                sb.Append(@"/");

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
                sb.Append(@"\");

                return sb.ToString();
            }
        }

        public DirectoryEntry()
        {
        }

        public DirectoryEntry(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public DirectoryEntry(string name, DirectoryEntry parent)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public void Add(DirectoryEntry folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            folder.Parent = this;
            directories.Add(folder);
        }

        public void Add(FileEntry file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            file.Parent = this;
            files.Add(file);
        }

        public bool Remove(DirectoryEntry folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            folder.Parent = null;
            return directories.Remove(folder);
        }

        public bool Remove(FileEntry file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            file.Parent = null;
            return files.Remove(file);
        }

        /// <summary>
        /// Gets all the items including the items of childs
        /// </summary>
        /// <returns>A list of items</returns>
        public IEnumerable<FileEntry> GetItems()
        {
            return Files.Concat(Directories.SelectMany(d => d.GetItems()));
        }
    }
}
