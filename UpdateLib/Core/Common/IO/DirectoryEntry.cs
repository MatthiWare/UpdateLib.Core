using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UpdateLib.Core.Common.IO
{
    public class DirectoryEntry
    {
        [JsonProperty(PropertyName = "Directories")]
        private List<DirectoryEntry> directories = new List<DirectoryEntry>();

        [JsonProperty(PropertyName = "Files")]
        private List<FileEntry> files = new List<FileEntry>();

        public string Name { get; set; }

        [JsonIgnore]
        public int Count => Files.Count + Directories.Sum(d => d.Count);

        [JsonIgnore]
        public IReadOnlyList<DirectoryEntry> Directories => directories.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyList<FileEntry> Files => files.AsReadOnly();

        public string Path { get; set; }

        public DirectoryEntry() { }

        public DirectoryEntry(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Path = $"{Name}\\";
        }

        public void Add(DirectoryEntry folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            folder.Path = $"{Path}{folder.Name}\\";

            directories.Add(folder);
        }

        public void Add(FileEntry file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            file.Path = $"{Path}{file.Name}";

            files.Add(file);
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
