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
    public class ArticleBuilder : ContentBuilderBase
    {
        public Catalogue<Article> ArticleCatalogue { get; set; } = new Catalogue<Article>();

        public ArticleBuilder()
        {
            ArticleCatalogue.Items = new List<Article>();
        }

        public void Build(string contentFolderLocation)
        {
            string[] directories = Directory.GetDirectories(contentFolderLocation, "*", SearchOption.TopDirectoryOnly);

            string articlesFolder = directories.Where(x => x.Contains("Articles")).FirstOrDefault();

            ArticleCatalogue.GeneratedOn = DateTime.UtcNow;

            ArticleCatalogue.Items = GetArticles(articlesFolder);

            SaveCatalogue();
        }

        internal List<Article> GetArticles(string currentFolderLocation)
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

                contentItems.Add(new Article
                {
                    Category = regexGroups["Category"].Value.Replace(@"\", "/") ?? "",
                    Created = file.Created,
                    LastUpdated = file.LastUpdated,
                    Title = contentConfig["Title"],
                    File = regexGroups["Filename"].Value,
                    Lede = contentConfig["Lede"]
                });
            }

            return contentItems;
        }

        internal override void SaveCatalogue()
        {
            Directory.CreateDirectory(@"build\catalogues");
            FileStream fileStream = File.Create(@"build\catalogues\ArticleCatalogue.json");
            fileStream.Dispose();
            fileStream.Close();

            StreamWriter sw = new StreamWriter(@"build\catalogues\ArticleCatalogue.json", false, Encoding.UTF8);
            sw.Write(
                JsonSerializer.Serialize(ArticleCatalogue)
                );
            sw.Dispose();
            sw.Close();
        }
    }
}
