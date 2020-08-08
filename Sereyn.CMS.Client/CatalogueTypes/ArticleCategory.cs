using Sereyn.CMS.Entities;
using System.Collections.Generic;

namespace Sereyn.CMS.Catalogues.Models
{
    public class ArticleCategory : CatalogueItem
    {
        public List<ArticleCategory> SubCategories { get; set; }

    }
}
