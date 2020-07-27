using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sereyn.CMS.CatalogueBuilder.Models
{
    public class ContentFileInfo
    {
        public string File { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public static ContentFileInfo For(string file)
        {
            FileInfo fileInfo = new FileInfo(file);

            return new ContentFileInfo
            {
                File = file,
                Created = fileInfo.CreationTimeUtc,
                LastUpdated = fileInfo.LastWriteTimeUtc
            };
        }
    }
}
