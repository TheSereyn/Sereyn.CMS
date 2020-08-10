using Sereyn.CMS.CatalogueBuilder.Builders;
using System;
using System.IO;

namespace Sereyn.CMS.CatalogueBuilder
{
    class Program
    {
        private static string _contentDirectory;
        private static string _catalogueDirectory;

        static int Main(string[] args)
        {
            // Check the arguments have been set. 
            if(args.Length == 0)
            {
                Console.WriteLine("Please provide the required arguments.");
                Console.WriteLine("Example: CatalogueBuilder <ContentDirectory> <CatalogueDirectory>");
                return 1;
            }

            // Check the content directory exits.
            if(string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Content Directory hasn't been specified.");
                return 1;
            }
            else
            {
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Input content directory can not be found.");
                    Console.WriteLine("If the path includes spaces then try encasing the path in double quotes and trying again.");
                    return 1;
                }

                _contentDirectory = args[0];
            }

            // Check if a catalogue directory has been provided
            if(args.Length == 2)
            {
                if (!Directory.Exists(args[1]))
                {
                    Console.WriteLine("Catalogue directory not found. Creating directory...");
                    _catalogueDirectory = Directory.CreateDirectory(args[1]).FullName;
                }

                _catalogueDirectory = args[1];
            }
            else
            {
                Console.WriteLine("Catalogue directory hasn't been specified. Defaulting to the following directory: ");

                DirectoryInfo directoryInfo = new DirectoryInfo(_contentDirectory);
                _catalogueDirectory = directoryInfo.Parent.CreateSubdirectory("Catalogues").FullName;

                Console.WriteLine(string.Format("\t {0}", _catalogueDirectory));
            }
            
            ArticlesCatalogueFactory articlesCatalogueFactory = new ArticlesCatalogueFactory(_contentDirectory);

            articlesCatalogueFactory.GetCatalogue().Save(_catalogueDirectory);
            articlesCatalogueFactory.GetCatagoriesCatalogue().Save(_catalogueDirectory);

            new StaticPagesCatalogueFactory(_contentDirectory).GetCatalogue().Save(_catalogueDirectory);

            

            return 0;
        }
    }
}
