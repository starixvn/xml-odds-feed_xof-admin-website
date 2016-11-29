namespace Fanex.OddsFeed.Common.DTO
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class Outright
    {
        public int EventId { get; set; }

        public IEnumerable<Language> EventNames { get; set; }

        public DateTime Time { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public string CountryCode { get; set; }

        public IList<OutrightOdds> OutrightOddsList { get; set; }
    }
}
