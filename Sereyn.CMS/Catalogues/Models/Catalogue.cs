using System;
using System.Collections.Generic;

namespace Sereyn.CMS.Catalogues.Models
{
    public class Catalogue<T>
    {
        public DateTime GeneratedOn { get; set; }
        public List<T> Items { get; set; }
    }
}
