using System;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    public interface IUpdateCatalogStorage : IStorage<UpdateCatalogFile>
    {
        bool Exists { get; }
        DateTime LastWriteTime { get; }
    }
}
