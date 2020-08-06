using Sereyn.CMS.Catalogues.Models;
using System.Threading.Tasks;

namespace Sereyn.CMS.Interfaces
{
    public interface ICatalogueManager
    {
        Task<Catalogue<T>> GetCatalogueAsync<T>();
    }
}
