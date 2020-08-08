using Sereyn.CMS.Interfaces;
using Sereyn.CMS.ValueObjects;

namespace Sereyn.CMS.Entities
{
    public class CatalogueItem : ICatalogueItem
    {
        public string Route { get; set; }
        public string Name { get; set; }
        public ContentFileInfo FileInfo { get; set; }
    }
}
