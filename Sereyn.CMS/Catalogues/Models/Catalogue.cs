using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Sereyn.CMS.Catalogues.Models
{
    public class Catalogue<T>
    {
        public DateTime GeneratedOn { get; set; }
        public List<T> Items { get; set; }

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
