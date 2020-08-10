using Microsoft.Extensions.Options;
using Sereyn.CMS.Entities;
using Sereyn.CMS.Interfaces;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sereyn.CMS.Client
{
    public class CatalogueManager : ICatalogueManager
    {
        #region Members

        private readonly ClientOptions _clientOptions;

        #endregion

        #region Constructor

        public CatalogueManager(IOptions<ClientOptions> clientOptionsAccessor)
        {
            _clientOptions = clientOptionsAccessor.Value;
        }

        #endregion

        #region Methods

        public async Task<Catalogue<T>> GetCatalogueAsync<T>() where T : ICatalogueItem
        {
            Catalogue<T> catalogue = await JsonSerializer.DeserializeAsync<Catalogue<T>>(
                await GetCatalogueHttpStreamAsync(
                    string.Format("{0}/{1}",
                        _clientOptions.CatalogueFolder,
                        Catalogue<T>.FileName
                        )
                    )
                );

            return catalogue;
        }

        private async Task<Stream> GetCatalogueHttpStreamAsync(string catalogueFile)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new System.Uri(_clientOptions.BaseUrl)
            };

            HttpResponseMessage response = await http.GetAsync(catalogueFile);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new FileNotFoundException("Unable to find requested catalogue file.", catalogueFile);
            else 
                return await response.Content.ReadAsStreamAsync();
        }

        #endregion
    }
}
