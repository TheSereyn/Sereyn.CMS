using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Sereyn.CMS.Entities
{
    public class Catalogue<T> where T : CatalogueItem
    {
        public DateTime GeneratedOn { get; private set; }
        public List<T> Items { get; private set; }

        public Catalogue()
        {
            GeneratedOn = DateTime.UtcNow;
            Items = new List<T>();
        }

        public void AddItem(T catalogueItem)
        {
            Items.Add(catalogueItem);
        }

        public void Save(string directoryPath)
        {
            Type catalogueType = typeof(T);

            StreamWriter sw = new StreamWriter(
                string.Format("{0}{1}{2}Catalogue.json", directoryPath, Path.DirectorySeparatorChar, catalogueType.Name), 
                false, 
                Encoding.UTF8
                );

            sw.Write(
                JsonSerializer.Serialize(this)
                );
            sw.Dispose();
            sw.Close();
        }
    }
}
