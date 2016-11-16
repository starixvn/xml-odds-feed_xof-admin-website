namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Library;
    using Library.Configurations;
    using Library.Nodes;

    public class AnalyticsController : Controller
    {
        /// <summary>
        /// Caches this instance.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult Cache()
        {
            var totalVersion = int.Parse(ConfigurationManager.AppSettings.Get("TotalVersion"));
            string result = string.Format("<h3>Current version: {0}, Full chain: {1}</h3>", Caching.GetCurrentVersion(), Caching.IsFullChain());
            string cachePrefix = AppSettings.CachePrefix;

            for (var version = 0; version < totalVersion; version++)
            {
                var matches = Caching.Get(CacheKeys.DeadballMatchKey(cachePrefix, version)) as IList<Match>;
                var outrights = Caching.Get(CacheKeys.OutrightKey(cachePrefix, version)) as IList<Outright>;

                int matchCount = matches == null ? 0 : matches.Count;
                int outrightCount = outrights == null ? 0 : outrights.Count;

                result += string.Format("<h3>Version: {0}, match: {1}, event: {2}</h3>", version, matchCount, outrightCount);
            }

            ViewBag.Result = result;
            return View();
        }

        /// <summary>
        /// Emails this instance.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult Email()
        {
            return View();
        }

        /// <summary>
        /// Emails the specified to.
        /// </summary>
        /// <param name="to">To email address.</param>
        /// <param name="content">The content.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult Email(string to, string content)
        {
            // ViewBag.Sent = DNAide.Utils.SendMailService.SendMail(
            //                    ConfigurationManager.AppSettings["SMTPServer"],
            //                    to,
            //                    ConfigurationManager.AppSettings["FromMail"],
            //                    string.Empty,
            //                    "Odds feed services: Email sending testing",
            //                    content, 25, null
            //                );
            ViewBag.Sent = Utilities.SendEmail(ConfigurationManager.AppSettings["FromMail"], to, "Odds feed services: Email sending testing", content);
            return View();
        }
    }
}
