using Sereyn.CMS.Entities;
using System.Collections.Generic;

namespace Sereyn.CMS.Client.CatalogueTypes
{
    public class ArticleCategory : CatalogueItem
    {
        public List<ArticleCategory> SubCategories { get; set; }

    }
}
