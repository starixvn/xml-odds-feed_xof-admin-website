namespace Fanex.OddsFeed.WebServiceLibrary.Models
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(TypeName = "Countries")]
    public class Country
    {
        public string CountryId { get; set; }

        public string Name_en { get; set; }

        public string Name_cs { get; set; }

        public string Name_ct { get; set; }
    }
}
