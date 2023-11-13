namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using Fanex.OddsFeed.Core.Web.Context;
    using Fanex.OddsFeed.Core.Web.Session;
    using Microsoft.AspNetCore.Mvc;
    [IsAuthorize]
    public class AffiliateController : BaseController
    {
        private const string MajorGroup = "Major";
        private const string MinorGroup = "Minor";
        private const string URLPrefix = "http://";
        private const string DefaultController = "Affiliate";
        private const string OddsRegionSettingType = "OddsRegion";
        private const string OddsTypeSettingType = "OddsType";
        private const byte OutrightDelayTimesIndex = 2;

        private readonly IAffiliateService affiliateService;
        private readonly ILicenseeService licenseeService;

        public AffiliateController(ICurrentContext currentContext, 
            ICurrentSession currentSession, 
            IBaseService baseService,
            string feedUrlPath, 
            string virtualPathWebService) : base(currentContext, currentSession, baseService, feedUrlPath, virtualPathWebService)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
