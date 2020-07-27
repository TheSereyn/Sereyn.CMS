using Microsoft.Extensions.Configuration;
using Sereyn.CMS.CatalogueBuilder.Models;
using Sereyn.CMS.Catalogues.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Sereyn.CMS.CatalogueBuilder.Builders
{
    public class PageBuilder : ContentBuilderBase
    {
        public Catalogue<Page> PageCatalogue { get; set; } = new Catalogue<Page>();

        public PageBuilder()
        {
            PageCatalogue.Items = new List<Page>();
        }

        public void Build(string contentFolderLocation)
        {
            string[] directories = Directory.GetDirectories(contentFolderLocation, "*", SearchOption.TopDirectoryOnly);

            string pagesFolder = directories.Where(x => x.Contains("Pages")).FirstOrDefault();

            PageCatalogue.GeneratedOn = DateTime.UtcNow;
            PageCatalogue.Items = GetPages(pagesFolder);

            SaveCatalogue();
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

                if (regexGroups["Category"].Value != "")
                {
                    fileLocation = string.Format("Content/{0}/{1}/{2}.md",
                        regexGroups["Type"].Value,
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
                }

                contentItems.Add(new Page
                {
                    Created = file.Created,
                    LastUpdated = file.LastUpdated,
                    Title = contentConfig["Title"],
                    File = fileLocation
                });
            }

            return contentItems;
        }

        internal override void SaveCatalogue()
        {
            Directory.CreateDirectory(@"build\catalogues");
            FileStream fileStream = File.Create(@"build\catalogues\PageCatalogue.json");
            fileStream.Dispose();
            fileStream.Close();

            StreamWriter sw = new StreamWriter(@"build\catalogues\PageCatalogue.json", false, Encoding.UTF8);
            sw.Write(
                JsonSerializer.Serialize(PageCatalogue)
                );
            sw.Dispose();
            sw.Close();
        }

        

    }
}
