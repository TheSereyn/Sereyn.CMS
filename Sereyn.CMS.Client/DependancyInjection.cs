using Microsoft.Extensions.DependencyInjection;
using Sereyn.CMS.Interfaces;
using System;

namespace Sereyn.CMS.Client
{
    public static class DependancyInjection
    {
        //public static IServiceCollection AddSereynCMS(this IServiceCollection services)
        //{
        //    services.AddTransient<IContentManager, ContentManager>();
        //    services.AddTransient<ICatalogueManager, CatalogueManager>();

        //    return services;
        //}

        public static IServiceCollection AddSereynCMS(this IServiceCollection services, Action<ClientOptions> clientOptions)
        {
            services.AddTransient<IContentManager, ContentManager>();
            services.AddTransient<ICatalogueManager, CatalogueManager>();

            services.Configure(clientOptions);

            return services;
        }
    }
}
