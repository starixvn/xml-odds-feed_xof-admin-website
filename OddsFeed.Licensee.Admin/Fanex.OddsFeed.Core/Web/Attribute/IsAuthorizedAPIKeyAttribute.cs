namespace Fanex.OddsFeed.Core.Web.Attribute
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    [AttributeUsage(AttributeTargets.Method)]
    public class IsAuthorizedAPIKeyAttribute : AuthorizeAttribute
    {
        private string ApiKeyXOF = ConfigurationManager.AppSettings["ApiKey"];
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            const string HeaderKeyName = "ApiKey";
            var apiKey = actionContext.Request.Headers.GetValues(HeaderKeyName);

            if (apiKey == null || !IsValidApiKey(apiKey.FirstOrDefault()))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Content = new StringContent("<html><body><h1>API Key not found or incorrect</h1></body></html>", Encoding.UTF8, "text/html");

                actionContext.Response = response;
            }
        }

        private bool IsValidApiKey(string apiKey)
        {
            return apiKey == ApiKeyXOF;
        }
    }
}
