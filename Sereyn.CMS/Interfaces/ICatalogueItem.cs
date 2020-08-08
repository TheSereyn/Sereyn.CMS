using Sereyn.CMS.ValueObjects;

namespace Sereyn.CMS.Interfaces
{
    public interface ICatalogueItem
    {
        string Route { get; set; }
        string Name { get; set; }
        ContentFileInfo FileInfo { get; set; }
    }
}
