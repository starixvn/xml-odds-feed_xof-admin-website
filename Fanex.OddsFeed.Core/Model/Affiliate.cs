namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class Affiliate
    {
        public int Row { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public short SystemID { get; set; }

        public short ProfileID { get; set; }

        public string Site { get; set; }

        public bool IsAllowed { get; set; }

        public string Remark { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public DateTime LastRequest { get; set; }

        public string Creator { get; set; }

        public string ContactPerson { get; set; }

        public string ContactEmail { get; set; }
    }
}