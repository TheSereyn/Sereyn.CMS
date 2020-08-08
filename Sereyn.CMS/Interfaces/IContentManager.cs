using Sereyn.CMS.ValueObjects;
using System.Threading.Tasks;

namespace Sereyn.CMS.Interfaces
{
    public interface IContentManager
    {
        Task<Content> GetContentAsync(string requestedContentFile);
    }
}
