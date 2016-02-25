using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nexcel.OddsFeed.WebService.Sports
{
    [XmlRoot("sports")]
    public class SportsNode
    {
        [XmlElement("sport")]
        public List<SportNode> SportList { get; set; }
    }
}