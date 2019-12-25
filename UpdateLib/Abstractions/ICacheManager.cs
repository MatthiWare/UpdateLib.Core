using System.Threading.Tasks;

namespace UpdateLib.Abstractions
{
    interface ICacheManager
    {
        Task UpdateCacheAsync();
    }
}
