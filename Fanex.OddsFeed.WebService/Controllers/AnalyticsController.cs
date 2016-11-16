namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Library;

    /// <summary>
    /// Class AnalyticsController.
    /// </summary>
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
            var prefix = ConfigurationManager.AppSettings["Cache.Prefix"];
            for (var i = 0; i < totalVersion; i++)
            {
                //var match = Caching.Get(prefix + "deadballmatches" + i) as IList<Match>;
                //var _event = Caching.Get(prefix + "events" + i) as IList<Event>;

                //var m = match == null ? 0 : match.Count;
                //var e = _event == null ? 0 : _event.Count;

                //result += string.Format("<h3>Version: {0}, match: {1}, event: {2}</h3>", i, m, e);
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
