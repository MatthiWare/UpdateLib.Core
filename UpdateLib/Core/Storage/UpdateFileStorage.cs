using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using UpdateLib.Abstractions.Storage;
using UpdateLib.Core.Storage.Files;
using static System.Environment;

namespace UpdateLib.Core.Storage
{
    public class UpdateFileStorage : IUpdateFileStorage
    {
        private const string UpdateFileName = "UpdateInfo.json";

        private readonly IFileSystem fs;
        private readonly string updateFilePath;

        public UpdateFileStorage(IFileSystem fs)
        {
            this.fs = fs ?? throw new ArgumentNullException(nameof(fs));

            updateFilePath = GetFilePathAndEnsureCreated();
        }

        private string GetFilePathAndEnsureCreated()
        {
            // Use DoNotVerify in case LocalApplicationData doesn’t exist.
            string path = fs.Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData, SpecialFolderOption.DoNotVerify), "UpdateLib", UpdateFileName);
            // Ensure the directory and all its parents exist.
            fs.Directory.CreateDirectory(path);

            return path;
        }

        public async Task<UpdateFile> LoadAsync()
        {
            using (var reader = fs.File.OpenText(updateFilePath))
            {
                var contents = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<UpdateFile>(contents);
            }
        }

        public async Task SaveAsync(UpdateFile file)
        {
            using (var stream = fs.File.OpenWrite(updateFilePath))
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
