using Microsoft.AspNetCore.Components;
using Sereyn.CMS.Catalogues;
using Sereyn.CMS.Catalogues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sereyn.CMS.Examples.Blog.Shared
{
    public partial class NavMenu
    {
        #region Members

        private List<NavItem> navItems = new List<NavItem>();

        #endregion

        #region Overrides

        protected override void OnInitialized()
        {
            GenerateNav();
        }

        #endregion

        #region Methods

        private async void GenerateNav()
        {
            Catalogue<ArticleCategory> categoryCatalogue = await CatalogueManager.GetCatalogueAsync<ArticleCategory>();

            foreach (var item in categoryCatalogue.Items)
            {
                navItems.Add(new NavItem { 
                    Name = item.Name,
                    Route = string.Format("Articles/{0}", item.Route)
                });
            }

            StateHasChanged();
        }

        #endregion

        #region Properties

        [Inject] private ICatalogueManager CatalogueManager { get; set; }

        #endregion
    }

    public class NavItem
    {
        public string Name { get; set; }
        public string Route { get; set; }
    }
}
