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
    class ArticlesCatalogueFactory : CatalogueFactory<Article>
    {
        private string _contentDirectory;

        public ArticlesCatalogueFactory(string contentDirectory)
        {
            _contentDirectory = contentDirectory;
        }

        public override Catalogue<Article> GetCatalogue()
        {
            Catalogue<Article> articlesCatalogue = new Catalogue<Article>();            

            string[] directories = Directory.GetDirectories(_contentDirectory, "*", SearchOption.TopDirectoryOnly);
            string articlesFolder = directories.Where(x => x.Contains("Articles")).FirstOrDefault();

            articlesCatalogue.GeneratedOn = DateTime.UtcNow;

            articlesCatalogue.Items = GetArticles(articlesFolder);

            return articlesCatalogue;
        }

        public Catalogue<ArticleCategory> GetCatagoriesCatalogue()
        {
            Catalogue<ArticleCategory> categoriesCatalogue = new Catalogue<ArticleCategory>();

            string[] directories = Directory.GetDirectories(_contentDirectory, "*", SearchOption.TopDirectoryOnly);

            categoriesCatalogue.GeneratedOn = DateTime.UtcNow;
            string articlesFolder = directories.Where(x => x.Contains("Articles")).FirstOrDefault();
            categoriesCatalogue.Items = GetCategories(articlesFolder);

            return categoriesCatalogue;
        }

        private List<Article> GetArticles(string currentFolderLocation)
        {
            List<ContentFileInfo> contentFiles = GetContentFiles(currentFolderLocation);

            List<Article> contentItems = new List<Article>();

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

                contentItems.Add(new Article
                {
                    Category = regexGroups["Category"].Value.Replace(@"\", "/") ?? "",
                    Created = file.Created,
                    LastUpdated = file.LastUpdated,
                    Title = contentConfig["Title"],
                    File = fileLocation,
                    Lede = contentConfig["Lede"]
                });
            }

            return contentItems;
        }

        private List<ArticleCategory> GetCategories(string currentDirectory)
        {
            string[] directories = Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly);
            List<ArticleCategory> categories = new List<ArticleCategory>();

            if (directories.Length != 0)
            {
                foreach (var directory in directories)
                {
                    Regex regex = new Regex(
                        @"^.*[\\/]*content[\\/]{1}(?<Type>[a-zA-Z0-9]*)[\\/]{0,1}(?<Category>[a-zA-Z0-9\s\\/\-]*)$",
                        RegexOptions.IgnoreCase);

                    GroupCollection regexGroups = regex.Match(directory).Groups;

                    string category = regexGroups["Category"].Value.Replace(@"\", "/") ?? "";

                    categories.Add(new ArticleCategory
                    {
                        Name = category.Split(new string[] { "/" }, StringSplitOptions.None).Last(),
                        Route = category.Replace(" ", "-"),
                        SubCategories = GetCategories(directory)
                    });
                }

            }

            return categories;
        }
    }
}
