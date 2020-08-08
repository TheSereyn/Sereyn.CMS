using Microsoft.Extensions.Configuration;
using Sereyn.CMS.Common;
using System.Collections.Generic;
using System.IO;

namespace Sereyn.CMS.ValueObjects
{
    public class Content : BaseValueObject
    {
        #region Methods

        public static Content GetContentFromStream(Stream content)
        {
            StreamReader reader = new StreamReader(content);

            return new Content
            {
                Configuration = GetConfiguration(content, out reader),
                RawMarkdown = reader.ReadToEnd()
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
                    return new string(chars.ToArray());
                }
            }

            throw new InvalidDataException("Json configuration missing or invalid in content file.");
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
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Configuration;
            yield return RawMarkdown;
        }

        #endregion

        #region Properties

        public IConfiguration Configuration { get; private set; }
        public string RawMarkdown { get; private set; }

        #endregion




    }
}
