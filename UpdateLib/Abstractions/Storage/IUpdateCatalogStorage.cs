using System;
using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    public interface IUpdateCatalogStorage
    {
        bool Exists { get; }
        DateTime LastWriteTime { get; }
        Task<UpdateCatalogFile> LoadAsync();
        Task SaveAsync(UpdateCatalogFile file);
    }
}
