namespace Fanex.OddsFeed.Common.DTO
{
    using global::System;
    using global::System.Collections.Generic;

    [Serializable]
    public class Outright
    {
        public int EventId { get; set; }

        public string EventEnglish { get; set; }

        public string EventTraditionalChinese { get; set; }

        public string EventSimplifiedChinese { get; set; }

        public DateTime Time { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public IList<OutrightOdds> OutrightOddsList { get; set; }
    }
}
