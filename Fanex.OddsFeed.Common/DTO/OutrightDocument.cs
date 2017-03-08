namespace Fanex.OddsFeed.Common.DTO
{
    using System.Collections.Generic;

    public class OutrightDocument
    {
        public int Version { get; set; }

        public IList<Outright> Outrights { get; set; }

        //public int EventId { get; set; }

        //public string EventEn { get; set; }

        //public string EventCn { get; set; }

        //public string EventTw { get; set; }

        //public DateTime Time { get; set; }

        //public int SportId { get; set; }

        //public string SportName { get; set; }

        //public string CountryCode { get; set; }

        //public SafeList<OutrightOdds> OutrightOddsList { get; set; }
    }
}
