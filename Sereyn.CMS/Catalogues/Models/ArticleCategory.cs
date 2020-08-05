using System;
using System.Collections.Generic;
using System.Text;

namespace Sereyn.CMS.Catalogues.Models
{
    public class ArticleCategory
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public List<ArticleCategory> SubCategories { get; set; }
    }
}
