namespace Fanex.OddsFeed.Library.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class OddsFeedBuilder
    {
        protected IDictionary<string, string> _leagueAttributes = new Dictionary<string, string>
        {
            { "ge", "en-US" },
            { "gt", "zh-TW" },
            { "gs", "zh-CN" }
        };

        protected IDictionary<string, string> _homeAttributes = new Dictionary<string, string>
        {
            { "he", "en-US" },
            { "ht", "zh-TW" },
            { "hs", "zh-CN" }
        };

        protected IDictionary<string, string> _awayAttibutes = new Dictionary<string, string>
        {
            { "ae", "en-US" },
            { "at", "zh-TW" },
            { "as", "zh-CN" }
        };

        protected IDictionary<string, string> _teamAttributes = new Dictionary<string, string>
        {
            { "te", "en-US" },
            { "tt", "zh-TW" },
            { "ts", "zh-CN" }
        };

        protected void AssignNames(XmlDocument xmlDocument, XmlNode node, IEnumerable<Language> names, IDictionary<string, string> tagAttributes)
        {
            foreach (var tagAttribute in tagAttributes)
            {
                var attribute = xmlDocument.CreateAttribute(tagAttribute.Key);
                var value = names
                    .Where(name => string.Equals(name.Code, tagAttribute.Value))
                    .Select(name => name.Content).First();

                attribute.Value = Utilities.EscapeXML(value);
                node.Attributes.Append(attribute);
            }
        }

        protected void AssignId(XmlDocument xmlDocument, XmlNode node, int id)
        {
            var idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = id.ToString();
            node.Attributes.Append(idAttribute);
        }

        protected void AssignSport(XmlDocument xmlDocument, XmlNode node, string sportName)
        {
            var sportAttribute = xmlDocument.CreateAttribute("sport");
            sportAttribute.Value = sportName.ToString();
            node.Attributes.Append(sportAttribute);
        }

        protected void AssignMatchStatus(XmlDocument xmlDocument, XmlNode node, bool isLive)
        {
            var runningAttribute = xmlDocument.CreateAttribute("running");
            runningAttribute.Value = isLive ? "1" : "0";
            node.Attributes.Append(runningAttribute);
        }

        protected void AssignMatchKickOff(XmlDocument xmlDocument, XmlNode node, DateTime kickOffTime)
        {
            var mtAttribute = xmlDocument.CreateAttribute("mt");
            mtAttribute.Value = kickOffTime.ToString();
            node.Attributes.Append(mtAttribute);
        }
    }
}
