using Sereyn.CMS.Contents.Models;
using System.Threading.Tasks;

namespace Sereyn.CMS.Interfaces
{
    public interface IContentManager
    {
        Task<Content> GetContentAsync(string requestedContentFile);
    }
}
