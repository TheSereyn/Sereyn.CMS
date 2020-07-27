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
    internal static class CategoryBuilder
    {
        public static Catalogue<Category> CategoryCatalogue { get; set; } = new Catalogue<Category>();

        static CategoryBuilder()
        {
            CategoryCatalogue.Items = new List<Category>();
        }

        internal static void Build(string contentFolderLocation)
        {
            string[] directories = Directory.GetDirectories(contentFolderLocation, "*", SearchOption.TopDirectoryOnly);

            CategoryCatalogue.GeneratedOn = DateTime.UtcNow;
            //CategoryCatalogue.Items = GetCategories(contentFolderLocation);

            string articlesFolder = directories.Where(x => x.Contains("Articles")).FirstOrDefault();

            CategoryCatalogue.Items = GetCategories(articlesFolder);

            SaveCatalogue();
        }

        private static List<Category> GetCategories(string currentDirectory)
        {
            string[] directories = Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly);
            List<Category> categories = new List<Category>();

            if(directories.Length != 0)
            {
                foreach (var directory in directories)
                {
                    Regex regex = new Regex(
                        @"^.*[\\/]*content[\\/]{1}(?<Type>[a-zA-Z0-9]*)[\\/]{0,1}(?<Category>[a-zA-Z0-9\s\\/\-]*)$",
                        RegexOptions.IgnoreCase);

                    GroupCollection regexGroups = regex.Match(directory).Groups;

                    string category = regexGroups["Category"].Value.Replace(@"\", "/") ?? "";

                    categories.Add(new Category
                    {
                        Name = category.Split(new string[] { "/" }, StringSplitOptions.None).Last(),
                        Route = category.Replace(" ", "-"),
                        SubCategories = GetCategories(directory)
                    });
                }
                
            }

            return categories;
        }

        private static void SaveCatalogue()
        {
            Directory.CreateDirectory(@"build\catalogues");
            FileStream fileStream = File.Create(@"build\catalogues\CategoryCatalogue.json");
            fileStream.Dispose();
            fileStream.Close();

            StreamWriter sw = new StreamWriter(@"build\catalogues\CategoryCatalogue.json", false, Encoding.UTF8);
            sw.Write(
                JsonSerializer.Serialize(CategoryCatalogue)
                );
            sw.Dispose();
            sw.Close();
        }

    }
}
