using Sereyn.CMS.Catalogues.Models;
using System.Threading.Tasks;

namespace Sereyn.CMS.Catalogues
{
    public interface ICatalogueManager
    {
        Task<Catalogue<T>> GetCatalogueAsync<T>();
    }
}
