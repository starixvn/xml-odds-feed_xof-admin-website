namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.WebService.Common;
    using OddsFeed.Common.DTO;
    using WebServiceLibrary.Services;

    public class BetTypeController : Controller
    {
        private OddsFeedSerializer _serializer;
        private IBetTypeService _betTypeService;

        public BetTypeController()
        {
            _serializer = new OddsFeedSerializer();
            _betTypeService = new BetTypeService();
        }

        [Compress]
        [PartnerPermission]
        public ActionResult Index(string username, UserProfile userProfile)
        {
            var betTypes = GetBetTypes(username);
            LimitLanguage(betTypes, userProfile);

            var xml = _serializer.Serialize(betTypes);
            return Content(xml, "text/xml", Encoding.UTF8);
        }

        private BetTypesNode GetBetTypes(string username)
        {
            var criteria = new BetTypesCriteria { Username = username };
            var repository = new DynamicRepository();
            var rawBetTypes = repository.Fetch<RawBetType>(criteria).ToList();
            var betTypes = rawBetTypes.Select(r => FromRaw(r)).ToList();

            return new BetTypesNode
            {
                BetTypeList = betTypes
            };
        }

        private BetTypeNode FromRaw(RawBetType raw)
        {
            return new BetTypeNode
            {
                Id = raw.Id.ToString(),
                Type = _betTypeService.GetBetTypeNameById(raw.Id),
                NameEnglish = raw.NameEnglish,
                NameSimplifiedChinese = raw.NameSimplifiedChinese,
                NameTraditionalChinese = raw.NameTraditionalChinese
            };
        }

        private void LimitLanguage(BetTypesNode nodes, OddsProfile oddsProfile)
        {
            var enableEnglish = oddsProfile.Languages.Contains("en-US");
            var enableTraditionalChinese = oddsProfile.Languages.Contains("zh-TW");
            var enableSimplifiedChinese = oddsProfile.Languages.Contains("zh-CN");

            nodes.BetTypeList.ForEach(b =>
            {
                b.NameEnglish = enableEnglish ? b.NameEnglish : string.Empty;
                b.NameTraditionalChinese = enableTraditionalChinese ? b.NameTraditionalChinese : string.Empty;
                b.NameSimplifiedChinese = enableSimplifiedChinese ? b.NameSimplifiedChinese : string.Empty;
            });
        }
    }
}