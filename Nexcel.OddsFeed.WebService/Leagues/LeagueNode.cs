using System.Xml.Serialization;

namespace Nexcel.OddsFeed.WebService.Leagues
{
    public class LeagueNode
    {
        [XmlAttribute("id")]
        public string LeagueId { get; set; }

        [XmlAttribute("sportid")]
        public int SportId { get; set; }

        [XmlAttribute("sportname")]
        public string SportName { get; set; }

        [XmlAttribute("ge")]
        public string NameEnglish { get; set; }

        [XmlAttribute("gt")]
        public string NameTraditionalChinese { get; set; }

        [XmlAttribute("gs")]
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