using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Sereyn.CMS.Client.CatalogueTypes;
using Sereyn.CMS.Entities;
using Sereyn.CMS.ValueObjects;

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

            articlesCatalogue = GetArticles(articlesFolder, articlesCatalogue);

            return articlesCatalogue;
        }

        public Catalogue<ArticleCategory> GetCatagoriesCatalogue()
        {
            Catalogue<ArticleCategory> categoriesCatalogue = new Catalogue<ArticleCategory>();

            string[] directories = Directory.GetDirectories(_contentDirectory, "*", SearchOption.TopDirectoryOnly);

            string articlesFolder = directories.Where(x => x.Contains("Articles")).FirstOrDefault();
            categoriesCatalogue = GetCategories(articlesFolder, categoriesCatalogue);

            return categoriesCatalogue;
        }

        private Catalogue<Article> GetArticles(string currentFolderLocation, Catalogue<Article> catalogue)
        {
            List<ContentFileInfo> contentFiles = GetContentFiles(currentFolderLocation);

            foreach (var file in contentFiles)
            {
                IConfiguration contentConfig = OpenContentFile(file.FileLocation);

                catalogue.AddItem(new Article
                {
                    Name = contentConfig["Title"],
                    FileInfo = file,
                    Lede = contentConfig["Lede"]
                });
            }

            return catalogue;
        }

        private Catalogue<ArticleCategory> GetCategories(string currentDirectory, Catalogue<ArticleCategory> catalogue)
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

                    catalogue.AddItem(new ArticleCategory
                    {
                        Name = category.Split(new string[] { "/" }, StringSplitOptions.None).Last(),
                        Route = category.Replace(" ", "-"),
                        SubCategories = GetCategories(directory)
                    });
                }

            }

            return catalogue;
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
