namespace Fanex.OddsFeed.Common.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public static class SerializerHelper
    {
        public static string SerializeList<T>(IEnumerable<T> list)
        {
            if (list == null || list.Count() == 0)
            {
                return string.Empty;
            }

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, GetXmlWriterSettings()))
            {
                var serializer = new XmlSerializer(list.GetType());
                serializer.Serialize(xmlWriter, list, GetXmlSerializerNamespaces());
                return stringWriter.ToString();
            }
        }

        private static XmlSerializerNamespaces GetXmlSerializerNamespaces()
        {
            var xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add(string.Empty, string.Empty);
            return xmlNameSpace;
        }

        private static XmlWriterSettings GetXmlWriterSettings()
        {
            return new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None
            };
        }
    }
}
