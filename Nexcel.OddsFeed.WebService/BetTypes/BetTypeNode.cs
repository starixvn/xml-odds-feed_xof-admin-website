using System.Xml.Serialization;

namespace Nexcel.OddsFeed.WebService.BetTypes
{
    public class BetTypeNode
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("te")]
        public string NameEnglish { get; set; }

        [XmlAttribute("tt")]
        public string NameTraditionalChinese { get; set; }

        [XmlAttribute("ts")]
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