using Newtonsoft.Json;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;
using static System.Environment;

namespace UpdateLib.Core.Storage
{
    public class UpdateCatalogStorage : IUpdateCatalogStorage
    {
        private readonly IFileSystem fs;
        private readonly string catalogFilePath;

        public UpdateCatalogStorage(IFileSystem fs)
        {
            this.fs = fs ?? throw new System.ArgumentNullException(nameof(fs));

            catalogFilePath = GetFilePathAndEnsureCreated();
        }

        private string GetFilePathAndEnsureCreated()
        {
            // Use DoNotVerify in case LocalApplicationData doesn’t exist.
            string path = fs.Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData, SpecialFolderOption.DoNotVerify), "UpdateLib", "Cache", "UpdateCatalogus.json");
            // Ensure the directory and all its parents exist.
            fs.Directory.CreateDirectory(path);

            return path;
        }

        public async Task<UpdateCatalogFile> LoadAsync()
        {
            using (var reader = fs.File.OpenText(catalogFilePath))
            {
                var contents = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<UpdateCatalogFile>(contents);
            }
        }

        public async Task SaveAsync(UpdateCatalogFile file)
        {
            using (var stream = fs.File.OpenWrite(catalogFilePath))
            using (var writer = new StreamWriter(stream))
            {
                // truncate
                stream.SetLength(0);

                var contents = JsonConvert.SerializeObject(file);
                await writer.WriteAsync(contents);
            }
        }
    }
}
