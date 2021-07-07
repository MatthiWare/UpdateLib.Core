using System;
using System.IO.Abstractions;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Core.Storage
{
    public class UpdateCatalogStorage : BaseStorage<UpdateCatalogFile>, IUpdateCatalogStorage
    {
        private const string FileName = "UpdateCatalogus.json";

        public bool Exists => FileInfo.Exists;

        public DateTime LastWriteTime => FileInfo.LastWriteTimeUtc;

        public UpdateCatalogStorage(IFileSystem fs)
            : base(fs, "UpdateLib", "Cache", FileName)
        {
        }
    }
}
