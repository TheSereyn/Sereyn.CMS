using Microsoft.Extensions.Configuration;
using Sereyn.CMS.Contents.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sereyn.CMS.Contents
{
    public class ContentManager : IContentManager
    {
        #region Members

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public ContentManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task<Content> GetContentAsync(string requestedContentFile)
        {
            Console.WriteLine("Content File: " + requestedContentFile);
            return Content.GetContentFromStream(
                await GetContentStreamAsync(requestedContentFile));
        }

        private async Task<Stream> GetContentStreamAsync(string contentFileLocation)
        {
            HttpClient http = new HttpClient
            {
                BaseAddress = new System.Uri(_configuration["SereynCMS:BaseUrl"])
            };

            HttpResponseMessage response = await http.GetAsync(
                string.Format("{0}",
                contentFileLocation));

            return await response.Content.ReadAsStreamAsync();
        }

        #endregion
    }
}
