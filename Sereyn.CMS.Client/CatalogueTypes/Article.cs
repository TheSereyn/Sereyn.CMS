﻿using Sereyn.CMS.Entities;

namespace Sereyn.CMS.Client.CatalogueTypes
{
    public class Article : CatalogueItem
    {
        public string Category { get; set; }
        public string Lede { get; set; }
    }
}