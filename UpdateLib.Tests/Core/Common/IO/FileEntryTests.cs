using Xunit;
using UpdateLib.Core.Common.IO;

namespace UpdateLib.Tests.Core.Common.IO
{
    public class FileEntryTests
    {
        [Fact]
        public void ShouldGiveCorrectSourceAndDestination()
        {
            DirectoryEntry root = new DirectoryEntry("%root%");
            DirectoryEntry subFolder = new DirectoryEntry("sub");
            FileEntry file = new FileEntry("myfile.txt");

            root.Add(subFolder);

            subFolder.Add(file);

            string outputDest = "%root%\\sub\\myfile.txt";

            Assert.Equal(outputDest, file.Path);
        }
    }
}
