using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Nexcel.OddsFeed.WebService.Common
{
    public class OddsFeedSerializer
    {
        public string Serialize(object @object)
        {
            var xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add(string.Empty, string.Empty);
            var serializer = new XmlSerializer(@object.GetType());

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    serializer.Serialize(streamWriter, @object, xmlNamespace);
                }

                var utf8EncodedXml = memoryStream.ToArray();
                return Encoding.UTF8.GetString(utf8EncodedXml);
            }
        }
    }
}