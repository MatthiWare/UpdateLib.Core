using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using static System.Environment;

namespace UpdateLib.Abstractions.Storage
{
    public abstract class BaseStorage<TFile> : IStorage<TFile>
    {
        private readonly string localPath;
        private readonly IFileSystem fs;

        public BaseStorage(IFileSystem fs, string appName, string dirName, string fileName)
        {
            this.fs = fs ?? throw new System.ArgumentNullException(nameof(fs));

            localPath = GetFilePathAndEnsureCreated(appName, dirName, fileName);

            FileInfo = fs.FileInfo.FromFileName(localPath);
        }

        protected string GetFilePathAndEnsureCreated(string appName, string dirName, string fileName)
        {
            // Use DoNotVerify in case LocalApplicationData doesn’t exist.
            string path = fs.Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData, SpecialFolderOption.DoNotVerify), appName, dirName, fileName);
            // Ensure the directory and all its parents exist.
            fs.Directory.CreateDirectory(fs.Path.GetDirectoryName(path));

            return path;
        }

        public IFileInfo FileInfo { get; protected set; }

        public virtual async Task<TFile> LoadAsync()
        {
            if (!FileInfo.Exists)
                throw new FileNotFoundException("File not found", FileInfo.Name);

            using (var reader = fs.File.OpenText(localPath))
            {
                var contents = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<TFile>(contents);
            }
        }

        public virtual async Task SaveAsync(TFile file)
        {
            if (FileInfo.Exists && FileInfo.IsReadOnly)
                throw new InvalidOperationException($"Writing to read-only file is not allowed '{FileInfo.FullName}'");

            using (var stream = fs.File.OpenWrite(localPath))
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
