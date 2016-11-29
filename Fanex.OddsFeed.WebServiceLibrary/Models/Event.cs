namespace Fanex.OddsFeed.WebServiceLibrary.Models
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(TypeName = "Events")]
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
