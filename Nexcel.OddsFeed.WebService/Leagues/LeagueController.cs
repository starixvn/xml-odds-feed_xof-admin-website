using Fanex.Data.Repository;
using Nexcel.OddsFeed.Library;
using Nexcel.OddsFeed.Library.Attributes;
using Nexcel.OddsFeed.Library.Matchs.OddNodeBuilder;
using Nexcel.OddsFeed.WebService.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nexcel.OddsFeed.WebService.Leagues
{
    public class LeagueController : Controller
    {
        private OddsFeedSerializer serializer = new OddsFeedSerializer();

        [Compress]
        [PartnerPermission]
        public ActionResult Index(string username, OddsProfile oddsProfile)
        {
            var leagues = GetLeagues(username);
            LimitLanguage(leagues, oddsProfile);

            var xml = serializer.Serialize(leagues);
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        private LeaguesNode GetLeagues(string username)
        {
            var criteria = new LeaguesCriteria { Username = username };
            var repository = new DynamicRepository();
            var leagues = repository.Fetch<LeagueNode>(criteria).ToList();
            EscapeLeagues(leagues);
            return new LeaguesNode
            {
                LeagueList = leagues
            };
        }

        private void EscapeLeagues(List<LeagueNode> leagues)
        {
            foreach (LeagueNode league in leagues)
            {
                league.NameEnglish = BetOddNodeBuilder.EscapeXML(league.NameEnglish);
                league.NameTraditionalChinese = BetOddNodeBuilder.EscapeXML(league.NameTraditionalChinese);
                league.NameSimplifiedChinese = BetOddNodeBuilder.EscapeXML(league.NameSimplifiedChinese);
            }
        }

        private void LimitLanguage(LeaguesNode nodes, OddsProfile oddsProfile)
        {
            var enableEnglish = oddsProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = oddsProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = oddsProfile.Languages.Contains("zh-CN");

            nodes.LeagueList.ForEach(b =>
            {
                b.NameEnglish = enableEnglish ? b.NameEnglish : string.Empty;
                b.NameTraditionalChinese = enableTraditionalChinese ? b.NameTraditionalChinese : string.Empty;
                b.NameSimplifiedChinese = enableSimplifiedChinese ? b.NameSimplifiedChinese : string.Empty;
            });
        }
    }
}