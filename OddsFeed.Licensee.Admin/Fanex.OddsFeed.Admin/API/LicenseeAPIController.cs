namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Web.Attribute;
    public class LicenseeAPIController : BaseController
    {
        [IsAuthorizedAPIKey]
        [HttpPost]
        public ActionResult EditActive(string name)
        {
            var criteria = new UpdateLicenseeActiveCriteria
            {
                IP = GetClientIP(),
                IsActive = false,
                SystemID = 227
            };

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetClientIP()
        {
            var clientIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrWhiteSpace(clientIp))
            {
                var forwardedIps = clientIp.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                clientIp = forwardedIps[forwardedIps.Length - 1];
            }
            else
            {
                clientIp = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (clientIp == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }
    }
}