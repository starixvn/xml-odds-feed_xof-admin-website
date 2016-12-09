namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Common.DTO;
    using Fanex.OddsFeed.WebService.Common;
    using Sports;
    using WebServiceLibrary.Attributes;

    public class SportController : Controller
    {
        private OddsFeedSerializer serializer = new OddsFeedSerializer();

        [PartnerPermission]
        public ActionResult Index(string username, UserProfile userProfile)
        {
            var sports = GetSports(username);
            LimitLanguage(sports, userProfile);

            var xml = serializer.Serialize(sports);
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        private static SportsNode GetSports(string username)
        {
            var criteria = new SportsCriteria { Username = username };
            var repository = new DynamicRepository();
            var sports = repository.Fetch<SportNode>(criteria).ToList();
            return new SportsNode
            {
                SportList = sports
            };
        }

        private static void LimitLanguage(SportsNode nodes, UserProfile userProfile)
        {
            var enableEnglish = userProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = userProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = userProfile.Languages.Contains("zh-CN");

            nodes.SportList.ForEach(b =>
            {
                b.NameEnglish = enableEnglish ? b.NameEnglish : string.Empty;
                b.NameTraditionalChinese = enableTraditionalChinese ? b.NameTraditionalChinese : string.Empty;
                b.NameSimplifiedChinese = enableSimplifiedChinese ? b.NameSimplifiedChinese : string.Empty;
            });
        }
    }
}