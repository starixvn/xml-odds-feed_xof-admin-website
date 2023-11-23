namespace Fanex.OddsFeed.Models
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
        public DateTime LastUpdate { get; set; }
    }
}