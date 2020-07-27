using System;
using System.Collections.Generic;
using System.Text;

namespace Sereyn.CMS.Catalogues.Models
{
    public class Category
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public List<Category> SubCategories { get; set; }
    }
}
