namespace Fanex.OddsFeed.Models.ValueObject
{
    using System;

    [Serializable]
    public class PartnerIP : IEntity<int, string>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IPFrom { get; set; }

        public string IPTo { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string ISP { get; set; }
    }
}