using Microsoft.Extensions.Configuration;
using Sereyn.CMS.Entities;
using Sereyn.CMS.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sereyn.CMS.Client.CatalogueTypes;

namespace Sereyn.CMS.CatalogueBuilder.Builders
{
    class StaticPagesCatalogueFactory : CatalogueFactory<StaticPage>
    {
        private string _contentDirectory;

        public StaticPagesCatalogueFactory(string contentDirectory)
        {
            _contentDirectory = contentDirectory;
        }

        public override Catalogue<StaticPage> GetCatalogue()
        {
            Catalogue<StaticPage> StaticPagesCatalogue = new Catalogue<StaticPage>();

            string[] directories = Directory.GetDirectories(_contentDirectory, "*", SearchOption.TopDirectoryOnly);

            string StaticPagesFolder = directories.Where(x => x.Contains("StaticPages")).FirstOrDefault();

            StaticPagesCatalogue = GetStaticPages(StaticPagesFolder, StaticPagesCatalogue);

            return StaticPagesCatalogue;
        }

        private Catalogue<StaticPage> GetStaticPages(string currentFolderLocation, Catalogue<StaticPage> catalogue)
        {
            List<ContentFileInfo> contentFiles = GetContentFiles(currentFolderLocation);

            foreach (var file in contentFiles)
            {
                IConfiguration contentConfig = OpenContentFile(file.FileLocation);

                string route = "";

                if (!string.IsNullOrEmpty(file.FileCategory))
                {
                    route = string.Format("{0}/{1}",
                        file.FileCategory,
                        file.FileName
                        );
                }
                else
                {
                    route = string.Format("{0}",
                        file.FileName
                        );
                }

                catalogue.AddItem(new StaticPage
                {
                    Name = contentConfig["Title"],
                    Route = route,
                    FileInfo = file
                });
            }

            return catalogue;
        }
    }
}
