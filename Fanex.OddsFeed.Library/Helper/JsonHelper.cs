namespace Fanex.OddsFeed.Library.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Logging;
    using Newtonsoft.Json;
    using Schemas;

    public class JsonHelper
    {
        public static IList<BetTypeSchema> GetBetTypeSchemas(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                return JsonConvert.DeserializeObject<List<BetTypeSchema>>(File.ReadAllText(fullPath));
            }
            catch (Exception exception)
            {
                var message = string.Format("Read BetType.json unsuccessful - Message: {0}", exception.Message);

                LogService.Error(message);
            }

            return null;
        }
    }
}
