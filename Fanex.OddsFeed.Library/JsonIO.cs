namespace Fanex.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.OddsFeed.Library.Schemas;
    using Newtonsoft.Json;

    public static class JsonIO
    {
        public static IList<BetTypeSchema> GetBetTypeSchemas()
        {
            var schemaPath = Path.Combine(Environment.CurrentDirectory, @"BetTypeSchema\BetType.json");

            return JsonConvert.DeserializeObject<List<BetTypeSchema>>(File.ReadAllText(schemaPath));
        }
    }
}
