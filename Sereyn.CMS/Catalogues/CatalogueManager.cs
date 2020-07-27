using Microsoft.Extensions.Configuration;
using Sereyn.CMS.Catalogues.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sereyn.CMS.Catalogues
{
    public class CatalogueManager : ICatalogueManager
    {
        #region Members

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public CatalogueManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task<Catalogue<T>> GetCatalogueAsync<T>()
        {
            return await JsonSerializer.DeserializeAsync<Catalogue<T>>(
                await GetCatalogueHttpStreamAsync(
                    string.Format("{0}/{1}Catalogue.json",
                        _configuration["SereynCMS:Catalogues:Folder"],
                        typeof(T).Name
                        )
                    )
                );
        }

        private async Task<Stream> GetCatalogueHttpStreamAsync(string catalogueFile)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["SereynCMS:BaseUrl"])
            };

            HttpResponseMessage response = await http.GetAsync(catalogueFile);
            return await response.Content.ReadAsStreamAsync();
        }

        #endregion
    }
}
