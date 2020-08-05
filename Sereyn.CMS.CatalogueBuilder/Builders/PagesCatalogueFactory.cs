using Microsoft.Extensions.Configuration;
using Sereyn.CMS.CatalogueBuilder.Models;
using Sereyn.CMS.Catalogues.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sereyn.CMS.CatalogueBuilder.Builders
{
    class PagesCatalogueFactory : CatalogueFactory<Page>
    {
        private string _contentDirectory;

        public PagesCatalogueFactory(string contentDirectory)
        {
            _contentDirectory = contentDirectory;
        }

        public override Catalogue<Page> GetCatalogue()
        {
            Catalogue<Page> PagesCatalogue = new Catalogue<Page>();

            string[] directories = Directory.GetDirectories(_contentDirectory, "*", SearchOption.TopDirectoryOnly);

            string pagesFolder = directories.Where(x => x.Contains("Pages")).FirstOrDefault();

            PagesCatalogue.GeneratedOn = DateTime.UtcNow;
            PagesCatalogue.Items = GetPages(pagesFolder);

            return PagesCatalogue;
        }

        private List<Page> GetPages(string currentFolderLocation)
        {
            List<ContentFileInfo> contentFiles = GetContentFiles(currentFolderLocation);

            List<Page> contentItems = new List<Page>();

            foreach (var file in contentFiles)
            {
                Regex regex = new Regex(
                    @"^.*[\\/]*content[\\/]{1}(?<Type>[a-zA-Z0-9]*)[\\/]{0,1}(?<Category>[a-zA-Z0-9\s\\/\-]*)[\\/]{1}(?<Filename>[a-zA-Z0-9\s\-]*)\.md$",
                    RegexOptions.IgnoreCase);

                GroupCollection regexGroups = regex.Match(file.File).Groups;

                IConfiguration contentConfig = OpenContentFile(file.File);
                string fileLocation = "";
                string route = "";

                if (regexGroups["Category"].Value != "")
                {
                    fileLocation = string.Format("Content/{0}/{1}/{2}.md",
                        regexGroups["Type"].Value,
                        regexGroups["Category"].Value.Replace(@"\", "/") ?? "",
                        regexGroups["Filename"].Value
                        );

                    route = string.Format("{0}/{1}",
                        regexGroups["Category"].Value.Replace(@"\", "/") ?? "",
                        regexGroups["Filename"].Value
                        );
                }
                else
                {
                    fileLocation = string.Format("Content/{0}/{1}.md",
                        regexGroups["Type"].Value,
                        regexGroups["Filename"].Value
                        );

                    route = string.Format("{0}",
                        regexGroups["Filename"].Value
                        );
                }

                contentItems.Add(new Page
                {
                    Created = file.Created,
                    LastUpdated = file.LastUpdated,
                    Title = contentConfig["Title"],
                    Route = route,
                    File = fileLocation
                });
            }

            return contentItems;
        }
    }
}
