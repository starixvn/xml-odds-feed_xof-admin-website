using System.Xml.Serialization;

namespace Fanex.OddsFeed.WebService.Sports
{
    public class SportNode
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("se")]
        public string NameEnglish { get; set; }

        [XmlAttribute("st")]
        public string NameTraditionalChinese { get; set; }

        [XmlAttribute("ss")]
        public string NameSimplifiedChinese { get; set; }

        [XmlIgnore]
        public bool NameEnglishSpecified
        {
            get
            {
                return !string.IsNullOrEmpty(NameEnglish);
            }
        }

        [XmlIgnore]
        public bool NameTraditionalChineseSpecified
        {
            get
            {
                return !string.IsNullOrEmpty(NameTraditionalChinese);
            }
        }

        [XmlIgnore]
        public bool NameSimplifiedChineseSpecified
        {
            get
            {
                return !string.IsNullOrEmpty(NameSimplifiedChinese);
            }
        }
    }
}