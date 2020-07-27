using Microsoft.Extensions.DependencyInjection;
using Sereyn.CMS.Catalogues;
using Sereyn.CMS.Contents;

namespace Sereyn.CMS
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSereynCMS(this IServiceCollection services)
        {
            services.AddTransient<IContentManager, ContentManager>();
            services.AddTransient<ICatalogueManager, CatalogueManager>();

            return services;
        }
    }
}
