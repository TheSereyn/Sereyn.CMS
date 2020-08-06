using Sereyn.CMS.ValueObjects;

namespace Sereyn.CMS.Entities
{
    public class CatalogueItem
    {
        public string Route { get; set; }
        public string Name { get; set; }
        public ContentFileInfo FileInfo { get; set; }
    }
}
