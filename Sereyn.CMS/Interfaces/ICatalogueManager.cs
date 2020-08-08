using Sereyn.CMS.Entities;
using System.Threading.Tasks;

namespace Sereyn.CMS.Interfaces
{
    public interface ICatalogueManager
    {
        Task<Catalogue<T>> GetCatalogueAsync<T>() where T : ICatalogueItem;
    }
}
