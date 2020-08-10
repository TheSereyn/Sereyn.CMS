using Microsoft.Extensions.Options;
using Sereyn.CMS.Interfaces;
using Sereyn.CMS.ValueObjects;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sereyn.CMS.Client
{
    public class ContentManager : IContentManager
    {
        #region Members

        private readonly ClientOptions _clientOptions;

        #endregion

        #region Constructor

        public ContentManager(IOptions<ClientOptions> clientOptionsAccessor)
        {
            _clientOptions = clientOptionsAccessor.Value;
        }

        #endregion

        #region Methods

        public async Task<Content> GetContentAsync(string requestedContentFile)
        {
            return Content.GetContentFromStream(
                await GetContentStreamAsync(requestedContentFile));
        }

        private async Task<Stream> GetContentStreamAsync(string contentFileLocation)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new System.Uri(_clientOptions.BaseUrl)
            };

            HttpResponseMessage response = await http.GetAsync(
                string.Format("{0}",
                contentFileLocation));

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new FileNotFoundException("Unable to find requested content file.", contentFileLocation);
            else
                return await response.Content.ReadAsStreamAsync();
        }

        #endregion
    }
}
