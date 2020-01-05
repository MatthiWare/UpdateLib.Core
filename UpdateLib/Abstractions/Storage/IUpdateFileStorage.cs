using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions.Storage
{
    interface IUpdateFileStorage
    {
        Task SaveAsync(UpdateFile file);
        Task<UpdateFile> LoadAsync();
    }
}
