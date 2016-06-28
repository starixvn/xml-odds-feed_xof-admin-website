using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fanex.OddsFeed.WebService.BetTypes
{
    [XmlRoot("bettypes")]
    public class BetTypesNode
    {
        [XmlElement("type")]
        public List<BetTypeNode> BetTypeList { get; set; }
    }
}