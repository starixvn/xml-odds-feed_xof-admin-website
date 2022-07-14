namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class Licensee
    {
        public int Row { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public short SystemID { get; set; }

        public string SystemName { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedDateToString => CreatedDate.ToString("dd MMM yyyy HH:mm:ss");

        public DateTime LastUpdate { get; set; }
        public string LastUpdateToString => LastUpdate.ToString("dd MMM yyyy HH:mm:ss");

        public int LicenseeID { get; set; }
        public string LicenseeName { get; set; }

        public bool IsActive { get; set; }

        public string ContactEmail { get; set; }
    }
}