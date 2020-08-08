using Sereyn.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sereyn.CMS.ValueObjects
{
    public class ContentFileInfo : BaseValueObject
    {
        public string FileName { get; private set; }
        public string FileLocation { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public static ContentFileInfo For(string file)
        {
            FileInfo fileInfo = new FileInfo(file);

            return new ContentFileInfo
            {
                FileName = fileInfo.Name,
                FileLocation = file,
                Created = fileInfo.CreationTimeUtc,
                LastUpdated = fileInfo.LastWriteTimeUtc
            };
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FileName;
            yield return FileLocation;
            yield return Created;
            yield return LastUpdated;
        }
    }
}
