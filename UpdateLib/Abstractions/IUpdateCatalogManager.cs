using System.Threading.Tasks;
using UpdateLib.Core;
using UpdateLib.Core.Common;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions
{
    public interface IUpdateCatalogManager
    {
        Task<UpdateCatalogFile> GetUpdateCatalogFileAsync();

        UpdateInfo GetLatestUpdateForVersion(UpdateVersion currentVersion, UpdateCatalogFile catalogFile);
    }
}
