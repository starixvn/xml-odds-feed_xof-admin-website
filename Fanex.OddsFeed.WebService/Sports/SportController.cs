using Fanex.Data.Repository;
using Fanex.OddsFeed.Library;
using Fanex.OddsFeed.Library.Attributes;
using Fanex.OddsFeed.WebService.Common;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Fanex.OddsFeed.WebService.Sports
{
    public class SportController : Controller
    {
        private OddsFeedSerializer serializer = new OddsFeedSerializer();

        [Compress]
        [PartnerPermission]
        public ActionResult Index(string username, OddsProfile oddsProfile)
        {
            var sports = GetSports(username);
            LimitLanguage(sports, oddsProfile);

            var xml = serializer.Serialize(sports);
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        private SportsNode GetSports(string username)
        {
            var criteria = new SportsCriteria { Username = username };
            var repository = new DynamicRepository();
            var sports = repository.Fetch<SportNode>(criteria).ToList();
            return new SportsNode
            {
                SportList = sports
            };
        }

        private void LimitLanguage(SportsNode nodes, OddsProfile oddsProfile)
        {
            var enableEnglish = oddsProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = oddsProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = oddsProfile.Languages.Contains("zh-CN");

            nodes.SportList.ForEach(b =>
            {
                b.NameEnglish = enableEnglish ? b.NameEnglish : string.Empty;
                b.NameTraditionalChinese = enableTraditionalChinese ? b.NameTraditionalChinese : string.Empty;
                b.NameSimplifiedChinese = enableSimplifiedChinese ? b.NameSimplifiedChinese : string.Empty;
            });
        }
    }
}