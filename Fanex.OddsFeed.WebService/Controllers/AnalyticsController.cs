namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using Common.Configurations;
    using WebServiceLibrary.Caching;
    using WebServiceLibrary.Helpers;

    public class AnalyticsController : Controller
    {
        public ActionResult Cache()
        {
            var totalVersion = AppSettings.TotalVersion;
            var currentVersion = OddFeedDataCachingOperator.GetCurrentVersion();
            var isFullChain = OddFeedDataCachingOperator.IsFullChain();
            var result = new StringBuilder();

            result.AppendFormat("<h3>Current version: {0}, Full chain: {1}</h3>", currentVersion, isFullChain);
            
            for (var version = 0; version < totalVersion; version++)
            {
                var matches = OddFeedDataCachingOperator.GetMatchs(version);
                var outrights = OddFeedDataCachingOperator.GetOutRight(version);

                var matchCount = matches == null ? 0 : matches.Count;
                var outrightCount = outrights == null ? 0 : outrights.Count;

                result.AppendFormat("<h3>Version: {0}, match: {1}, event: {2}</h3>", version, matchCount, outrightCount);
            }

            ViewBag.Result = result.ToString();

            return View();
        }

        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Email(string emailTo, string content)
        {
            var subject = "Odds feed services: Email sending testing";

            ViewBag.Sent = MailHelper.SendEmail(AppSettings.FromMail, emailTo, subject, content);

            return View();
        }
    }
}
