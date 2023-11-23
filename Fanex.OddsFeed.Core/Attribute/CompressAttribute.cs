namespace Fanex.OddsFeed.Core.Attribute
{
    using System.IO.Compression;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GZipEncodePage(context.HttpContext);
        }

        private bool IsGZipSupported(HttpRequest request)
        {
            var acceptEncoding = request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) &&
                    (acceptEncoding.ToString().Contains("gzip") || acceptEncoding.ToString().Contains("deflate")))
            {
                return true;
            }
            return false;
        }

        private void GZipEncodePage(HttpContext context)
        {
            var response = context.Response;

            if (IsGZipSupported(context.Request))
            {
                var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString();

                if (acceptEncoding.Contains("gzip"))
                {
                    response.Headers.Add("Content-Encoding", "gzip");
                    response.Body = new GZipStream(response.Body, CompressionLevel.Optimal, leaveOpen: true);
                }
                else
                {
                    response.Headers.Add("Content-Encoding", "deflate");
                    response.Body = new DeflateStream(response.Body, CompressionLevel.Optimal, leaveOpen: true);
                }
            }

            response.Headers.Append("Vary", "Content-Encoding");
        }
    }
}
