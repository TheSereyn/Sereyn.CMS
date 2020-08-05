using System;
using System.Collections.Generic;
using System.Text;

namespace Sereyn.CMS.Catalogues.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Lede { get; set; }
        public string Route
        {
            get
            {
                if (Category != "")
                {
                    return string.Format("{0}/{1}",
                    Category,
                    Title.Replace(" ", "-"));
                }
                else
                {
                    return string.Format("{0}",
                    Title.Replace(" ", "-"));
                }


            }
        }
        public string File { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
