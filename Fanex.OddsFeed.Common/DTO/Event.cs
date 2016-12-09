namespace Fanex.OddsFeed.Common.DTO
{
    using global::System.Xml.Serialization;

    [XmlRoot("Events")]
    public class Event
    {
        public int EventId { get; set; }

        public string EventName { get; set; }

        public string CountryId { get; set; }

        public string SportName { get; set; }

        public string EventDate { get; set; }

        public string KickOffTime { get; set; }

        public bool Status { get; set; }
    }
}
