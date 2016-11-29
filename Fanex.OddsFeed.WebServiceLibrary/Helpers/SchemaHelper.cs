namespace Fanex.OddsFeed.Library.Helpers
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
                using (var reader = new StreamReader(fullPath))
                {
                    var temp = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(temp))
                    {
                        return JsonConvert.DeserializeObject<List<BetTypeSchema>>(temp);
                    }
                    else
                    {
                        LogService.Info("BettypeSchema configuration is empty");

                    }
                }
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
