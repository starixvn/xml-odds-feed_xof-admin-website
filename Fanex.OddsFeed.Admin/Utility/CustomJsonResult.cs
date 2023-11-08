using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.OddsFeed.Admin.Utility
{
    public class CustomJsonResult : ObjectResult
    {
        private const string DateFormat = "dd MMM yyyy HH:mm:ss";

        public CustomJsonResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status200OK;
            ContentTypes = new MediaTypeCollection();
            ContentTypes.Add(ContentType ?? "application/json");
        }

        public string ContentType { get; set; }

        public Encoding ContentEncoding { get; set; }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            if (ContentEncoding != null)
            {
                response.Headers[HeaderNames.ContentType] = new StringValues($"{ContentType}; charset={ContentEncoding.WebName}");
            }
            else
            {
                response.Headers[HeaderNames.ContentType] = new StringValues(ContentType);
            }

            if (Value != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        NamingStrategy = new DefaultNamingStrategy()
                    }
                };
                settings.Converters.Add(new IsoDateTimeConverter
                {
                    DateTimeFormat = DateFormat
                });

                var json = JsonConvert.SerializeObject(Value, settings);
                response.WriteAsync(json);
            }
        }
    }

}
