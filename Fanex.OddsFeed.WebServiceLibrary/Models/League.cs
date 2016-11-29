namespace Fanex.OddsFeed.WebServiceLibrary.Models
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(TypeName = "Leagues")]
    public class League
    {
        public int LeagueId { get; set; }

        public string Name_en { get; set; }
        
        public string Name_cs { get; set; }

        public string Name_ct { get; set; }

        public string SportName { get; set; }

        public string CountryId { get; set; }

        public string StartDate { get; set; }

        public bool IsActive { get; set; }
    }
}
