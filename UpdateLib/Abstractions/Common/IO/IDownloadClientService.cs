using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Common.IO
{
    public interface IDownloadClientService
    {
        Task<UpdateFile> GetUpdateFileAsync();
        Task<UpdateCatalogFile> GetUpdateCatalogFileAsync();
    }
}
