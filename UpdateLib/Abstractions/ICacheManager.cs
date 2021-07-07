using System.Threading.Tasks;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Abstractions
{
    public interface ICacheManager
    {
        Task<HashCacheFile> UpdateCacheAsync();
    }
}
