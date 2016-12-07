namespace Fanex.OddsFeed.Common.DTO
{
    using System.Xml.Serialization;

    [XmlRoot("Countries")]
    public class Country
    {
        public string CountryId { get; set; }

        public string Name_en { get; set; }

        public string Name_cs { get; set; }

        public string Name_ct { get; set; }
    }
}
