using System;
using System.Collections.Generic;
using System.Text;

namespace Sereyn.CMS.Catalogues.Models
{
    public class Page
    {
        public string Title { get; set; }
        public string Route { get; set; }
        public string File { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
