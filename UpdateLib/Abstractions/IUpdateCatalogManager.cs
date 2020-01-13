using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions
{
    public interface IUpdateCatalogManager
    {
        Task<UpdateCatalogFile> GetUpdateCatalogFileAsync();
    }
}
