using Microsoft.Extensions.DependencyInjection;
using Sereyn.CMS.Interfaces;

namespace Sereyn.CMS.Client
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
