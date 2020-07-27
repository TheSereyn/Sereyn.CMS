﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sereyn.CMS.Contents.Models
{
    public class Content
    {
        #region Methods

        public static Content GetContentFromStream(Stream content)
        {
            StreamReader reader = new StreamReader(content);

            return new Content
            {
                Configuration = GetConfiguration(content, out reader),
                Markdown = reader.ReadToEnd()
            };
        }

        private static IConfiguration GetConfiguration(Stream content, out StreamReader reader)
        {
            return new ConfigurationBuilder().AddJsonStream(
                GenerateStream(
                    ExtractConfigurationFromContentStream(content, out reader)
                    )
            ).Build();
        }
        private static string ExtractConfigurationFromContentStream(Stream contentStream, out StreamReader reader)
        {
            reader = new StreamReader(contentStream);

            int OpenBracketCounter = 0;
            List<char> chars = new List<char>();

            while (reader.Peek() >= 0)
            {
                char c = (char)reader.Read();
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
                    break;
                }
            }

            return new String(chars.ToArray());
        }
        private static Stream GenerateStream(string jsonConfiguration)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(jsonConfiguration);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        #endregion

        #region Properties

        public IConfiguration Configuration { get; private set; }
        public string Markdown { get; private set; }

        #endregion




    }
}
