using Sereyn.CMS.Exceptions;
using Sereyn.CMS.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Sereyn.CMS.Entities
{
    public class Catalogue<T> where T : ICatalogueItem
    {
        public DateTime GeneratedOn { get; private set; }
        public List<T> Items { get; private set; }

        public static string FileName {
            get 
            {
                return string.Format("{0}-catalogue.json", typeof(T).Name.ToLower());
            }
        }

        public Catalogue()
        {
            GeneratedOn = DateTime.UtcNow;
            Items = new List<T>();
        }

        public void AddItem(T catalogueItem)
        {
            if(!Items.Exists(x => x.Route == catalogueItem.Route))
            {
                Items.Add(catalogueItem);
            }
            else
            {
                throw new DuplicateRouteException("Duplicate route found in catalogue.");
            }
        }

        public void Save(string directoryPath)
        {
            Type catalogueType = typeof(T);

            StreamWriter sw = new StreamWriter(
                string.Format("{0}{1}{2}", directoryPath, Path.DirectorySeparatorChar, FileName),
                false,
                Encoding.UTF8
                );

            sw.Write(
                JsonSerializer.Serialize(this)
                );
            sw.Dispose();
            sw.Close();
        }

        public T GetItem(string route)
        {
            return Items.Find(x => x.Route == route);
        }
    }
}
