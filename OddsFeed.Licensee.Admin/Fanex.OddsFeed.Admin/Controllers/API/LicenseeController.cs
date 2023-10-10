namespace Fanex.OddsFeed.Admin.Controllers.API
{
    using System.ServiceModel.Channels;
    using System.Web;
    using System.Web.Http;
    using Fanex.OddsFeed.Core.Criteria.Licensee;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using NLog;

    [ApiAuthorize]
    public class LicenseeController : ApiController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly IBaseService baseService;

        public LicenseeController() : this(null)
        {
        }

        public LicenseeController(
            IBaseService baseService)
        {
            this.baseService = baseService ?? new BaseService();
        }

        [HttpPost]
        public IHttpActionResult Deactive(string name)
        {
            WriteLogDeactiveLicensee(GetClientIP(), name);
            var criteria = new DeactiveLicenseeByNameCriteria
            {
                LicenseeName = name
            };

            CommandResult result = baseService.ExecuteNonQuery(criteria);
            return Json(result);
        }

        private string GetClientIP()
        {
            var clientIp = string.Empty;
            if (Request.Properties.ContainsKey("MS_HttpContext"))
            {
                clientIp = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }

            if (Request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                var prop = (RemoteEndpointMessageProperty)Request.Properties[RemoteEndpointMessageProperty.Name];
                clientIp = prop.Address;
            }

            if (clientIp == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }

        private void WriteLogDeactiveLicensee(string ip, string licenseeName)
        {
            logger.Info("IP: " + ip +
                   "\nAction: Deactive Licensee" +
                   "\nLicensee Name: " + licenseeName);
        }
    }
}