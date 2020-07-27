using Microsoft.AspNetCore.Components;
using Sereyn.CMS.Catalogues;
using Sereyn.CMS.Catalogues.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sereyn.CMS.Examples.Blog.Shared
{
    public partial class Featured
    {
        #region Members

        private List<Article> featuredArticles = new List<Article>();

        #endregion

        #region Overrides

        protected override void OnInitialized()
        {
            GetFeaturedContent();
        }

        #endregion

        #region Methods

        private async void GetFeaturedContent()
        {
            Catalogue<Article> articlesCatalogue = await CatalogueManager.GetCatalogueAsync<Article>();

            featuredArticles = articlesCatalogue.Items.OrderByDescending(x => x.Created).Take(3).ToList();


            StateHasChanged();
        }

        #endregion

        #region Properties

        [Inject] private ICatalogueManager CatalogueManager { get; set; }

        #endregion
    }
}
