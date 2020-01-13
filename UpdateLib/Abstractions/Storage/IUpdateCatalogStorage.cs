using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    public interface IUpdateCatalogStorage
    {
        Task<UpdateCatalogFile> LoadAsync();
        Task SaveAsync(UpdateCatalogFile file);
    }
}
