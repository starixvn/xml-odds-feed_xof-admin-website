namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.WebService.Common;
    using Leagues;
    using OddsFeed.Common.DTO;
    using WebServiceLibrary.Attributes;

    public class LeagueController : Controller
    {
        private OddsFeedSerializer serializer = new OddsFeedSerializer();

        [PartnerPermission]
        public ActionResult Index(string username, UserProfile userProfile)
        {
            var leagues = GetLeagues(username);
            LimitLanguage(leagues, userProfile);

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
                league.NameEnglish = league.NameEnglish;
                league.NameTraditionalChinese = league.NameTraditionalChinese;
                league.NameSimplifiedChinese = league.NameSimplifiedChinese;
            }
        }

        private void LimitLanguage(LeaguesNode nodes, UserProfile userProfile)
        {
            var enableEnglish = userProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = userProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = userProfile.Languages.Contains("zh-CN");

            nodes.LeagueList.ForEach(b =>
            {
                b.NameEnglish = enableEnglish ? b.NameEnglish : string.Empty;
                b.NameTraditionalChinese = enableTraditionalChinese ? b.NameTraditionalChinese : string.Empty;
                b.NameSimplifiedChinese = enableSimplifiedChinese ? b.NameSimplifiedChinese : string.Empty;
            });
        }
    }
}