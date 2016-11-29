namespace Fanex.OddsFeed.WebServiceLibrary.Attributes
{
    using System.IO.Compression;
    using System.Web.Mvc;

    public sealed class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            var zip = request["zip"] == null ? string.Empty : request["zip"].ToLower();
            switch (zip)
            {
                case "gzip":
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    return;
                case "deflate":
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    return;
            }
        }
    }
}
