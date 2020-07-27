using Markdig;
using Microsoft.AspNetCore.Components;
using Sereyn.CMS.Catalogues;
using Sereyn.CMS.Catalogues.Models;
using Sereyn.CMS.Contents;
using Sereyn.CMS.Contents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sereyn.CMS.Examples.Blog.Pages
{
    public partial class Index
    {
        #region Members

        private Content content;

        #endregion

        #region Overrides

        protected override void OnInitialized()
        {
            GetArticle();
        }

        #endregion

        #region Methods

        private async void GetArticle()
        {
            Catalogue<Article> catalogue = await CatalogueManager.GetCatalogueAsync<Article>();

            content = await ContentManager.GetContentAsync(
                catalogue.Items.OrderByDescending(x => x.Created).FirstOrDefault().File);

            StateHasChanged();
        }

        private MarkupString ContentHtml()
        {
            

            return new MarkupString(
                content.HtmlMarkup
                )
            );
        }

        #endregion

        #region Properties

        [Inject] private IContentManager ContentManager { get; set; }
        [Inject] private ICatalogueManager CatalogueManager { get; set; }
        #endregion
    }
}
