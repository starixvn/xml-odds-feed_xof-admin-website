namespace Fanex.OddsFeed.Core.Model
{
    using System;

    public class Domain
    {
        public int Row { get; set; }
        public short DomainId { get; set; }
        public string DomainName { get; set; }
        public string IP { get; set; }
        public string ISP { get; set; }
        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedDateToString => CreatedDate.ToString("dd MMM yyyy HH:mm:ss");

        public DateTime LastUpdate { get; set; }
        public string LastUpdateToString => LastUpdate.ToString("dd MMM yyyy HH:mm:ss");
    }
}