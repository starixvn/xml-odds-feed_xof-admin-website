namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class Licensee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public short SystemID { get; set; }

        public string SystemName { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public int LicenseeID { get; set; }

        public bool IsActive { get; set; }
    }
}