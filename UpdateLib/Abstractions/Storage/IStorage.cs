using System.IO.Abstractions;
using System.Threading.Tasks;

namespace UpdateLib.Abstractions.Storage
{
    public interface IStorage<TFile>
    {
        Task<TFile> LoadAsync();
        Task SaveAsync(TFile file);
    }
}
