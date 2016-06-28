using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fanex.OddsFeed.WebService.Leagues
{
    [XmlRoot("leagues")]
    public class LeaguesNode
    {
        [XmlElement("league")]
        public List<LeagueNode> LeagueList { get; set; }
    }
}