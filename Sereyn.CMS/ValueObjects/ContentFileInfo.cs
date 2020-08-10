using Sereyn.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Sereyn.CMS.ValueObjects
{
    public class ContentFileInfo : BaseValueObject
    {
        public string FileName { get; private set; }
        public string FileLocation { get; private set; }
        public string FileCategory { get; set; }
        public DateTime Created { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public static ContentFileInfo For(string file)
        {
            FileInfo fileInfo = new FileInfo(file);

            Regex regex = new Regex(
                    @"^.*[\\/]*content[\\/]{1}(?<Type>[a-zA-Z0-9]*)[\\/]{0,1}(?<Category>[a-zA-Z0-9\s\\/\-]*)[\\/]{1}(?<Filename>[a-zA-Z0-9\s\-]*)\.md$",
                    RegexOptions.IgnoreCase);

            GroupCollection regexGroups = regex.Match(fileInfo.FullName).Groups;

            if (!string.IsNullOrEmpty(regexGroups["Category"].Value))
            {
                string fileLocation = string.Format("Content/{0}/{1}/{2}.md",
                        regexGroups["Type"].Value,
                        regexGroups["Category"].Value.Replace(@"\", "/") ?? "",
                        regexGroups["Filename"].Value
                        );

                return new ContentFileInfo
                {
                    FileName = regexGroups["Filename"].Value,
                    FileLocation = fileLocation,
                    FileCategory = regexGroups["Category"].Value.Replace(@"\", "/") ?? "",
                    Created = fileInfo.CreationTimeUtc,
                    LastUpdated = fileInfo.LastWriteTimeUtc
                };
            }
            else
            {
                string fileLocation = string.Format("Content/{0}/{1}.md",
                        regexGroups["Type"].Value,
                        regexGroups["Filename"].Value
                        );

                return new ContentFileInfo
                {
                    FileName = regexGroups["Filename"].Value,
                    FileLocation = fileLocation,
                    Created = fileInfo.CreationTimeUtc,
                    LastUpdated = fileInfo.LastWriteTimeUtc
                };
            }
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
