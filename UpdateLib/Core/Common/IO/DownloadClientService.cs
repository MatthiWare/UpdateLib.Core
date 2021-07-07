using System;
using System.Net.Http;
using System.Threading.Tasks;
using UpdateLib.Abstractions.Common.IO;
using UpdateLib.Core.Storage.Files;

namespace UpdateLib.Core.Common.IO
{
    public class DownloadClientService : IDownloadClientService
    {
        private readonly HttpClient httpClient;

        public DownloadClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<UpdateCatalogFile> GetUpdateCatalogFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UpdateFile> GetUpdateFileAsync()
        {
            throw new NotImplementedException();
        }
    }
}
