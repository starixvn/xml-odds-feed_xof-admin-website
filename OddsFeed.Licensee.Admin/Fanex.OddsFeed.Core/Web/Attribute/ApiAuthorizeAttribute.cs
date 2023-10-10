namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string ApiKeyXOF = ConfigurationManager.AppSettings["ApiKey"];

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            const string HeaderKeyName = "ApiKey";
            var apiKey = actionContext.Request.Headers.GetValues(HeaderKeyName);

            if (apiKey == null || !IsValidApiKey(apiKey.FirstOrDefault()))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("API Key not found or incorrect", Encoding.UTF8, "text/html")
                };

                actionContext.Response = response;
            }
        }

        private bool IsValidApiKey(string apiKey)
        {
            return apiKey == ApiKeyXOF;
        }
    }
}