using Microsoft.Extensions.Configuration;
using Sereyn.CMS.Entities;
using Sereyn.CMS.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sereyn.CMS.Client
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

        public async Task<Catalogue<T>> GetCatalogueAsync<T>() where T : ICatalogueItem
        {
            Catalogue<T> catalogue = await JsonSerializer.DeserializeAsync<Catalogue<T>>(
                await GetCatalogueHttpStreamAsync(
                    string.Format("{0}/{1}",
                        _configuration["SereynCMS:Catalogues:Folder"],
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
                BaseAddress = new System.Uri(_configuration["SereynCMS:BaseUrl"])
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
