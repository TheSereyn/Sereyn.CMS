using Sereyn.CMS.Catalogues.Models;
using Microsoft.Extensions.Configuration;
using Sereyn.CMS.CatalogueBuilder.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sereyn.CMS.CatalogueBuilder.Builders
{
    abstract class CatalogueFactory<T>
    {
        public abstract Catalogue<T> GetCatalogue();

        internal List<ContentFileInfo> GetContentFiles(string contentFolderLocation)
        {
            List<ContentFileInfo> ContentFileInfos = new List<ContentFileInfo>();
            string[] files = Directory.GetFiles(contentFolderLocation, "*.md", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                ContentFileInfos.Add(
                    ContentFileInfo.For(file)
                    );
            }

            return ContentFileInfos;
        }

        internal IConfiguration OpenContentFile(string contentFile)
        {
            Stream contentStream = GetContentStreamAsync(contentFile);
            StreamReader contentStreamReader;

            IConfiguration contentConfig = GetContentConfiguration(contentStream, out contentStreamReader);

            contentStreamReader.Dispose();
            contentStreamReader.Close();

            contentStream.Dispose();
            contentStream.Close();

            return contentConfig;
        }

        private IConfiguration GetContentConfiguration(Stream contentStream, out StreamReader contentStreamReader)
        {
            return new ConfigurationBuilder()
                .AddJsonStream(
                    GenerateConfigurationStream(
                        ExtractConfigurationFromContentStream(contentStream, out contentStreamReader)
                        )
                    )
                .Build();
        }

        private Stream GetContentStreamAsync(string contentFileLocation)
        {
            return new StreamReader(contentFileLocation).BaseStream;
        }

        private string ExtractConfigurationFromContentStream(Stream contentStream, out StreamReader contentReader)
        {
            contentReader = new StreamReader(contentStream);

            int OpenBracketCounter = 0;
            List<char> chars = new List<char>();

            while (contentReader.Peek() >= 0)
            {
                char c = (char)contentReader.Read();
                chars.Add(c);

                if (c == '{')
                {
                    OpenBracketCounter++;
                }
                else if (c == '}')
                {
                    OpenBracketCounter--;
                }

                if (OpenBracketCounter == 0 && c == '}')
                {
                    return new String(chars.ToArray());
                }
            }

            throw new InvalidDataException("Invalid JSON");
        }

        private Stream GenerateConfigurationStream(string jsonConfiguration)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonConfiguration);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
