using Sereyn.CMS.CatalogueBuilder.Builders;
using System;

namespace Sereyn.CMS.CatalogueBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            string contentFolder = args[0] ?? Environment.CurrentDirectory;

            Console.WriteLine(contentFolder);

            new ArticleBuilder().Build(contentFolder);
            new PageBuilder().Build(contentFolder);
            CategoryBuilder.Build(contentFolder);
        }
    }
}
