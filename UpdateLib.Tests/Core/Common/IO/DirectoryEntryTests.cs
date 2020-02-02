using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateLib.Core.Common.IO;
using Xunit;

namespace UpdateLib.Tests.Core.Common.IO
{
    public class DirectoryEntryTests
    {
        [Fact]
        public void CountReturnsCorrectAmountOfFiles()
        {
            var root = CreateDirectoryWithFiles("1", 2);
            var dir2 = CreateDirectoryWithFiles("2", 0);
            var dir3 = CreateDirectoryWithFiles("3", 1);
            var dir4 = CreateDirectoryWithFiles("4", 4);
            var dir5 = CreateDirectoryWithFiles("5", 2);

            root.Add(dir2);
            root.Add(dir3);
            dir2.Add(dir4);
            dir3.Add(dir5);

            Assert.Equal(9, root.Count);
            Assert.Equal(9, root.GetItems().Count());
        }

        private DirectoryEntry CreateDirectoryWithFiles(string name, int filesToCreate)
        {
            var dir = new DirectoryEntry(name);

            for (int i = 0; i < filesToCreate; i++)
            {
                var entry = new FileEntry($"{name}.{i.ToString()}");

                dir.Add(entry);
            }

            return dir;
        }
    }
}
