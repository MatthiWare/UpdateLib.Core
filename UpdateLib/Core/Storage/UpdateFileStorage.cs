using System.IO.Abstractions;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Core.Storage
{
    public class UpdateFileStorage : BaseStorage<UpdateFile>, IUpdateFileStorage
    {
        private const string UpdateFileName = "UpdateInfo.json";

        public UpdateFileStorage(IFileSystem fs)
            : base(fs, "UpdateLib", string.Empty, UpdateFileName)
        {
        }
    }
}
